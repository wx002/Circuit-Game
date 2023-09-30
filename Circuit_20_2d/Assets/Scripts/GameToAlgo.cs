using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class GameToAlgo : MonoBehaviour
{

    //for parser-to-animator 
    private string fulllog;
    private bool thisDic;
    private int rowCount;
    private int colCount;
    private double voltMax;
    private List<List<string>> parsedLog;
    private List<string> nList;
    private Dictionary<string, double> current_dic;
    private Dictionary<string, double> voltage_dic;
    GameObject[][] grid;

    [System.Serializable]
    public struct SimObj
    {

        //material for the graph algorithms
        public GameObject auth;
        public GameObject p;
        public List<SimEdge> Adj;
        public int c;
        public int d;

        public SimObj(GameObject a1, GameObject p1, List<SimEdge> e1, int c1, int d1)
        {

            auth = a1;
            p = p1;
            Adj = e1;
            c = c1;
            d = d1;

        }

        public SimObj(GameObject a1)
        {

            auth = a1;
            p = a1.GetComponent<ComponentUpdate>().p;
            Adj = new List<SimEdge>();
            foreach(ComponentUpdate.Edge e in a1.GetComponent<ComponentUpdate>().Adj)
                Adj.Add(new SimEdge(e));
            c = a1.GetComponent<ComponentUpdate>().c;
            d = a1.GetComponent<ComponentUpdate>().d;

        }
    }

    [System.Serializable]
    public struct SimEdge
    {
        //object adjacent to this object
        public GameObject node;
        //the current flow
        public double flow;
        //the max flow
        public double cap;
        //special states of edge
        public Char ss;

        public SimEdge(ComponentUpdate.Edge e)
        {

            node = e.node;
            flow = e.flow;
            cap = e.cap;
            ss = e.ss;

        }

        public SimEdge(SimEdge e)
        {

            node = e.node;
            flow = e.flow;
            cap = e.cap;
            ss = e.ss;

        }
    }

    //builds array of game grid, and performs ops that need to be done to components
    //also calls sprice and gets results therefrom
    public void IterateGrid()
    {

        //gets standard filename set at startup
        string file_name = transform.parent.parent.GetComponent<SceneAttributes>().filePath;

        //deletes any old files
        if (File.Exists(file_name))
            File.Delete(file_name);

        thisDic = false;

        //gets size of game panel
        rowCount = transform.childCount;
        colCount = transform.GetComponentsInChildren<Transform>()[1].childCount;
        
        //instantiates the netlist to be passed to the SPICE handler
        nList = new List<string>();

        //get maximum volts for this scene
        voltMax = SearchUp("Foundation (UnityEngine.RectTransform)").gameObject.GetComponent<SceneAttributes>().voltS;

        //instantiates 2d grid of the game panel dimensions
        grid = new GameObject[rowCount][];
        for (int i = 0; i < rowCount; i++)
        {
            grid[i] = new GameObject[colCount];
        }

        //round one through the whole grid, filling it, updating front and back pointers, updates zeronode and building the nlist
        BuildNetlist(rowCount, colCount, grid, nList);

        //SPICE!
        SPICE(nList, file_name);
 
        //round two: uses output of spice to update nodes with voltage and components with voltage
        LogfileToGrid(rowCount, colCount, voltMax, grid);

        //round three: uses the state of the nodes to update voltage of the components
        FillGaps(rowCount, colCount, grid);
        
        //resets dic flag
        thisDic = false;

        //finds and calls animator
        SearchUp("GamePanel (UnityEngine.RectTransform)").GetComponent<Animator>().Tesla(grid, rowCount, colCount, voltMax);


    }

    private void BuildNetlist(int rowCount, int colCount, GameObject[][] grid, List<string> nList)
    {

        for (int i = 0; i < rowCount; i++)
        {

            Transform row = transform.Find(i.ToString());

            for (int j = 0; j < colCount; j++)
            {

                Transform col = row.Find(j.ToString());

                //checks if anything is in the cell
                if (col.childCount > 0)
                {

                    //gets the child and places it in the grid
                    Transform t = col.GetComponentsInChildren<Transform>()[1];
                    grid[i][j] = t.gameObject;
                    grid[i][j].GetComponent<Attributes>().strength = -1;

                    //resets the visited variable in object
                    grid[i][j].GetComponent<ComponentUpdate>().edgeDone = false;

                    //call something that updates next and prev for all components
                    if (t.gameObject.GetComponent<Attributes>().type != Attributes.Type.wire)
                        grid[i][j].GetComponent<ComponentUpdate>().UpdateComponent(grid[i][j].transform);

                    //call something that iterates through the node adjacent to the back of voltagesource
                    if (t.gameObject.GetComponent<Attributes>().back)
                        if (t.gameObject.GetComponent<Attributes>().type == Attributes.Type.vSource)
                            t.gameObject.GetComponent<Attributes>().back.GetComponent<ComponentUpdate>().UpdateZeroNode();

                    //Adds batteries and resistors to netlist
                    if (t.gameObject.GetComponent<Attributes>().back && t.gameObject.GetComponent<Attributes>().front)
                        if (t.gameObject.GetComponent<Attributes>().type == Attributes.Type.bResistor
                            || t.gameObject.GetComponent<Attributes>().type == Attributes.Type.mResistor
                            || t.gameObject.GetComponent<Attributes>().type == Attributes.Type.lResistor)
                        {

                            //build netlist entry
                            String thisString = t.gameObject.GetComponent<Attributes>().name + " "
                                + t.gameObject.GetComponent<Attributes>().front.GetComponent<Attributes>().name + " "
                                + t.gameObject.GetComponent<Attributes>().back.GetComponent<Attributes>().name + " "
                                + t.gameObject.GetComponent<Attributes>().resistance.ToString();

                            //add to netlist
                            nList.Add(thisString);

                        }
                    if (t.gameObject.GetComponent<Attributes>().back && t.gameObject.GetComponent<Attributes>().front)
                        if (t.gameObject.GetComponent<Attributes>().type == Attributes.Type.vSource)
                        {

                            //Build netlist entry
                            String thisString = t.gameObject.GetComponent<Attributes>().name + " "
                                + t.gameObject.GetComponent<Attributes>().front.GetComponent<Attributes>().name + " "
                                + t.gameObject.GetComponent<Attributes>().back.GetComponent<Attributes>().name + " "
                                + (t.gameObject.GetComponent<Attributes>().voltagePlus - t.gameObject.GetComponent<Attributes>().voltageMinus).ToString();

                            //add to netlist
                            nList.Add(thisString);

                        }

                }
                //there was no child, so grid gets a null space
                else
                {
                    grid[i][j] = null;
                }
            }

        }

    }

    private void SPICE(List<string> nList, String file_name)
    {

        //converts nList into netlist file
        SPICEController.generate_netlist(nList, ".OP");

        //Run SPICE
        if (File.Exists(SPICEController.netlist_path + "netlist.cir"))
            if (!(nList.Count == 0))
                SPICEController.call_SPICE("netlist.cir");

        //check for file's existence
        if (File.Exists(file_name))
        {

            //run parser
            fulllog = Parser.read_file(file_name);

            //check for keywords indicating a bad circuit
            if (!fulllog.Contains("singular") || !fulllog.Contains("shorted") || !fulllog.Contains("Assertion"))
            {

                //create list of lists to hold SPICE output
                List<List<string>> logList = Parser.parse_text(fulllog);

                //< 6 indicates garbage data
                if (logList.Count > 6)
                {

                    //further processes SPICE output
                    parsedLog = Parser.get_reduced_list(logList);

                    //checks for a proper parsed output to extract current data
                    if (!(parsedLog == null))
                        current_dic = Parser.get_component_data(parsedLog, Parser.DataType.CURRENT);

                    //checks for a proper component data to extract voltage data and flags dictionary as valid
                    if (!(current_dic == null))
                    {
                        voltage_dic = Parser.get_voltage(parsedLog);
                        thisDic = true;
                    }

                }

            }

        }

    }

    //Primary update of components
    private void LogfileToGrid(int rowCount, int colCount, double voltMax, GameObject[][] grid)
    {

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {

                if (grid[i][j] != null && thisDic && !(voltage_dic == null))
                {

                    if (grid[i][j].GetComponent<Attributes>().name[0] == 'r')
                    {

                        if (current_dic.ContainsKey(grid[i][j].GetComponent<Attributes>().name))
                        {

                            grid[i][j].GetComponent<Attributes>().amperage = current_dic[grid[i][j].GetComponent<Attributes>().name];

                            //turn on arrows
                            //these are placeholders for better current animation
                            if (grid[i][j].GetComponent<Attributes>().amperage > 0)
                            {

                                grid[i][j].GetComponentInChildren<Transform>().Find("AmpFore").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                                grid[i][j].GetComponentInChildren<Transform>().Find("AmpAft").gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

                            }
                            else if (grid[i][j].GetComponent<Attributes>().amperage < 0)
                            {

                                grid[i][j].GetComponentInChildren<Transform>().Find("AmpFore").gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                                grid[i][j].GetComponentInChildren<Transform>().Find("AmpAft").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

                            }
                            else
                            {

                                grid[i][j].GetComponentInChildren<Transform>().Find("AmpFore").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                                grid[i][j].GetComponentInChildren<Transform>().Find("AmpAft").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

                            }

                        }

                    }
                    else if (grid[i][j].GetComponent<Attributes>().name[0] == 'v')
                    {

                        if (current_dic.ContainsKey(grid[i][j].GetComponent<Attributes>().name))
                        {

                            grid[i][j].GetComponent<Attributes>().amperage = current_dic[grid[i][j].GetComponent<Attributes>().name];

                            //turn on arrows
                            //these are placeholders for better current animation
                            if (grid[i][j].GetComponent<Attributes>().amperage > 0)
                            {

                                grid[i][j].GetComponentInChildren<Transform>().Find("AmpFore").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                                grid[i][j].GetComponentInChildren<Transform>().Find("AmpAft").gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

                            }
                            else if (grid[i][j].GetComponent<Attributes>().amperage < 0)
                            {

                                grid[i][j].GetComponentInChildren<Transform>().Find("AmpFore").gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                                grid[i][j].GetComponentInChildren<Transform>().Find("AmpAft").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

                            }
                            else
                            {

                                grid[i][j].GetComponentInChildren<Transform>().Find("AmpFore").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                                grid[i][j].GetComponentInChildren<Transform>().Find("AmpAft").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

                            }

                        }

                    }
                    else if (grid[i][j].GetComponent<Attributes>().name[0] == 'n')
                    {
                        if (voltage_dic.ContainsKey(grid[i][j].GetComponent<Attributes>().name))
                        {

                            double vP = voltage_dic[grid[i][j].GetComponent<Attributes>().name];
                            double vM = voltage_dic[grid[i][j].GetComponent<Attributes>().name];

                            float pCol = (float)((vP / voltMax));
                            float mCol = (float)((vM / voltMax));

                            grid[i][j].GetComponent<Attributes>().voltagePlus = vP;
                            grid[i][j].GetComponent<Attributes>().voltageMinus = vM;

                        }
                        else
                        {

                            grid[i][j].GetComponent<Attributes>().voltagePlus = 0;
                            grid[i][j].GetComponent<Attributes>().voltageMinus = 0;

                        }

                    }

                }
                else if (grid[i][j] != null)
                {

                    //resets graph theory data
                    grid[i][j].GetComponent<ComponentUpdate>().edgeDone = false;
                    grid[i][j].GetComponent<ComponentUpdate>().ResetVertex();

                    grid[i][j].GetComponent<ComponentUpdate>().edgeDone = false;

                    if (grid[i][j].GetComponent<Attributes>().name[0] == 'r')
                    {

                        grid[i][j].GetComponent<Attributes>().amperage = 0;

                        grid[i][j].GetComponentInChildren<Transform>().Find("AmpFore").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                        grid[i][j].GetComponentInChildren<Transform>().Find("AmpAft").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

                    }
                    if (grid[i][j].GetComponent<Attributes>().name[0] == 'n')
                    {

                        grid[i][j].GetComponent<Attributes>().voltagePlus = 0;
                        grid[i][j].GetComponent<Attributes>().voltageMinus = 0;

                    }

                }

            }

        }

    }

    //secondary update of components
    private void FillGaps(int rowCount, int colCount, GameObject[][] grid)
    {

        for (int i = 0; i < rowCount; i++)
        {

            for (int j = 0; j < colCount; j++)
            {

                if (grid[i][j] != null)
                {

                    //call something that updates next and prev for all components and calculates node flow
                    if (grid[i][j].GetComponent<Attributes>().type != Attributes.Type.wire)
                        grid[i][j].GetComponent<ComponentUpdate>().UpdateComponent(grid[i][j].transform);

                    //call something that calculates flow through wires
                    else if(grid[i][j].GetComponent<Attributes>().type == Attributes.Type.wire)
                    {

                        if (!(grid[i][j].GetComponent<ComponentUpdate>().edgeDone))
                        {
                            
                            List<GameObject> V = new List<GameObject>()
                            {
                                grid[i][j].GetComponent<ComponentUpdate>().dumbSource,
                                grid[i][j].GetComponent<ComponentUpdate>().dumbSink
                            };

                            //grid[i][j].GetComponent<ComponentUpdate>().Edgy(V);

                            if (true)//I need a new gate
                            {

                            }

                            //resets graph theory data
                            foreach(GameObject v in V)
                            {
                                
                                v.GetComponent<ComponentUpdate>().ResetVertex();

                            }

                        }

                    }

                }

            }

        }

        //I include this because though not sure how to implement and integrate, I will need direction and magnitute for animations
        MB_connection_detector connection = ScriptableObject.CreateInstance<MB_connection_detector>();
        connection.Connection_Detector(grid, new string[2] { "Voltage source", "resistor 1" });
        connection.Find_Connections();
        //gameObject.GetComponent<MB_connection_detector>();
        //gameObject.GetComponent<MB_connection_detector>().Connection_Detector(grid, new string[2] { "Voltage_source", "resistor_1" });   

    }

    private List<SimObj> getRes(List<GameObject> V)
    {

        List<SimObj> resNet = new List<SimObj>();
        foreach (GameObject v in V)
        {

            List<SimEdge> edges = new List<SimEdge>();
            foreach (ComponentUpdate.Edge e in v.GetComponent<ComponentUpdate>().Adj)
            {

                SimEdge res = new SimEdge(e);
                edges.Add(res);

            }

        }

        return resNet;

    }

    //this will find a set of max flow paths from the sources to the sinks
    private void EdmondsKarp(GameObject source, GameObject t, List<GameObject> net)
    {

    }

    //this will be used by EdmondsKarp
    //algorithm adapted from Introduction to Algorithms, Third Edition 3rd, MIT press
    private void BFS(List<GameObject> V, GameObject s)
    {

        foreach (GameObject u in V)
        {

            u.GetComponent<ComponentUpdate>().c = 1;
            u.GetComponent<ComponentUpdate>().d = int.MaxValue;
            u.GetComponent<ComponentUpdate>().p = null;

        }

        s.GetComponent<ComponentUpdate>().c = 2;
        s.GetComponent<ComponentUpdate>().d = 0;
        s.GetComponent<ComponentUpdate>().p = null;

        Queue<GameObject> Q = new Queue<GameObject>();

        Q.Enqueue(s);

        while (Q.Count != 0)
        {

            GameObject u = Q.Dequeue();

            foreach (ComponentUpdate.Edge v in u.GetComponent<ComponentUpdate>().Adj)
            {

                if (v.node.GetComponent<ComponentUpdate>().c == 1)
                {

                    v.node.GetComponent<ComponentUpdate>().c = 2;
                    v.node.GetComponent<ComponentUpdate>().d = u.GetComponent<ComponentUpdate>().d + 1;
                    v.node.GetComponent<ComponentUpdate>().p = u;
                    Q.Enqueue(v.node);

                }

            }

            u.GetComponent<ComponentUpdate>().c = 3;

        }

    }
    //private void BFS(List<SimObj> V, SimObj s)
    

    //This helper function follows the hierarchy up until it reaches the ancestor called searchTo
    //I should add a safety catch in case of miss, and allow for passing in t, so other functions could use.
    Transform SearchUp(String searchTo)
    {

        Transform t = transform;
        while (!string.Equals(t.ToString(), searchTo))
        {

            t = t.parent;

        }

        return t;

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
