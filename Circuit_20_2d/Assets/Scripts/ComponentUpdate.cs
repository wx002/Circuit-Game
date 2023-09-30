using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ComponentUpdate : MonoBehaviour {

    private bool done;
    public bool edgeDone;

    public GameObject dumbSource;
    public GameObject dumbSink;

    //material for the graph algorithms
    public GameObject p;
    public List<Edge> Adj;
    public int c;
    public int d;

    [System.Serializable]
    public struct Edge
    {
        //object adjacent to this object
        public GameObject node;
        //the current flow
        public double flow;
        //the max flow
        public double cap;
        //special states of edge
        public Char ss;

        public Edge(GameObject n1, double f1, double c1, Char s1)
        {

            node = n1;
            flow = f1;
            cap = c1;
            ss = s1;

        }
    }

    //this actually ended up doing more than I intended.
    //this will rotate the sprites, set orientation, and set location
    public void UpdateOrientation(bool first)
    {

        //objects for up down left right
        GameObject[] rog = Rogers();
        GameObject atUp, atDn, atLf, atRt;

        atLf = rog[0];
        atRt = rog[1];
        atUp = rog[2];
        atDn = rog[3];

        //This just cleans things up a bit
        Transform p = transform.parent;

        //get names as strings
        String colStr = p.ToString().Split(' ')[0];
        String rowStr = p.parent.ToString().Split(' ')[0];

        //get names as ints
        int col;
        int.TryParse(colStr, out col);
        int row;
        int.TryParse(rowStr, out row);

        //sets location of GO
        gameObject.GetComponent<Attributes>().row = row;
        gameObject.GetComponent<Attributes>().col = col;

        //These are a casewise handling of each of the possible 16 states of surrounding objects
        //the pattern for the binary is LRUD
        //I don't think this can be abstracted
        if (!atLf && !atRt && !atUp && !atDn)//0000 -- 0 //This will be the case for all singles
        {

            //Straight Piece
            if(gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().IWire;
            //Orient Right
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 0);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.right;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            if(gameObject.GetComponent<Attributes>().type == Attributes.Type.vSource)
            {

                gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
                gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);

            }
            else
            {

                gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

            }
            
        }
        else if (atLf && !atRt && !atUp && !atDn)//1000 -- 1
        {

            //Straight Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().IWire;
            //Orient Right
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 180);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.right;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        }
        else if (!atLf && atRt && !atUp && !atDn)//0100 -- 1
        {

            //Straight Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().IWire;
            //Orient Right
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 0);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.right;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        }
        else if (atLf && atRt && !atUp && !atDn)//1100 -- 2
        {

            //Straight Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().IWire;
            //Orient Right
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 0);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.right;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        }
        else if (!atLf && !atRt && atUp && !atDn)//0010 -- 1
        {

            //Straight Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().IWire;
            //Orient Up
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 90);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.up;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        }
        else if (atLf && !atRt && atUp && !atDn)//1010 -- 2
        {

            //Elbow Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().LWire;
            //Orient up
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 180);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.up;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        }
        else if (!atLf && atRt && atUp && !atDn)//0110 -- 2
        {

            //Elbow Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().LWire;
            //Orient Left
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 90);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.left;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        }
        else if (atLf && atRt && atUp && !atDn)//1110 -- 3
        {

            //Tee Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().TWire;
            //Orient Left
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 180);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.left;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        }
        else if (!atLf && !atRt && !atUp && atDn)//0001 -- 1
        {

            //Straight Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().IWire;
            //Orient Down
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 270);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.down;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        }
        else if (atLf && !atRt && !atUp && atDn)//1001 -- 2
        {

            //Elbow Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().LWire;
            //Orient right
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 270);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.right;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        }
        else if (!atLf && atRt && !atUp && atDn)//0101 -- 2
        {

            //Elbow Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().LWire;
            //Orient down
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 0);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.down;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        }
        else if (atLf && atRt && !atUp && atDn)//1101 -- 3
        {

            //Tee Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().TWire;
            //Orient right
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 0);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.right;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        }
        else if (!atLf && !atRt && atUp && atDn)//0011 -- 2
        {

            //Straight Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().IWire;
            //Orient Up
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 90);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.up;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        }
        else if (atLf && !atRt && atUp && atDn)//1011 -- 3
        {

            //Tee Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().TWire;
            //Orient up
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 270);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.up;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        }
        else if (!atLf && atRt && atUp && atDn)//0111 -- 3
        {

            //Tee Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().TWire;
            //Orient down
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 90);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.down;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        }
        else if (atLf && atRt && atUp && atDn)//1111 -- 4
        {

            //X Piece
            if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
                gameObject.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<DragHandeler>().XWire;
            //Orient right
            transform.Rotate(0, 0, -transform.rotation.eulerAngles[2] + 0);
            gameObject.GetComponent<Attributes>().orient = Attributes.Orient.right;

            gameObject.GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
            gameObject.GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        }
        else
            print("WARNING: This should have been unreachable");

        //this uses the boolean parameter to update the surrounding
        if (first)
        {

            if(atLf)
                atLf.GetComponent<ComponentUpdate>().UpdateOrientation(false);
            if (atRt)
                atRt.GetComponent<ComponentUpdate>().UpdateOrientation(false);
            if (atUp)
                atUp.GetComponent<ComponentUpdate>().UpdateOrientation(false);
            if (atDn)
                atDn.GetComponent<ComponentUpdate>().UpdateOrientation(false);

        }

    }

    //this recursively calls neighbors until all connected nodes have the same name, generating a new name if none is given
    public void UpdateNode()
    {

        //gets a new name from foundation
        gameObject.GetComponent<Attributes>().name = SearchUp("Foundation (UnityEngine.RectTransform)").GetComponent<SceneAttributes>().GetNodeName();
        
        //objects for up down left right
        GameObject[] rog = Rogers();
        GameObject atUp, atDn, atLf, atRt;

        atLf = rog[0];
        atRt = rog[1];
        atUp = rog[2];
        atDn = rog[3];

        //flags the node as already updated
        gameObject.GetComponent<ComponentUpdate>().done = true;

        //updates adjacent wires with node name
        CheckAndUp(atLf, gameObject.GetComponent<Attributes>().name);
        CheckAndUp(atRt, gameObject.GetComponent<Attributes>().name);
        CheckAndUp(atUp, gameObject.GetComponent<Attributes>().name);
        CheckAndUp(atDn, gameObject.GetComponent<Attributes>().name);

    }
    public void UpdateNode(String newName)
    {

        gameObject.GetComponent<Attributes>().name = newName;

        //objects for up down left right
        GameObject[] rog = Rogers();
        GameObject atUp, atDn, atLf, atRt;

        atLf = rog[0];
        atRt = rog[1];
        atUp = rog[2];
        atDn = rog[3];

        //updates adjacent wires with node name
        gameObject.GetComponent<ComponentUpdate>().done = true;

        //updates adjacent wires with node name
        CheckAndUp(atLf, gameObject.GetComponent<Attributes>().name);
        CheckAndUp(atRt, gameObject.GetComponent<Attributes>().name);
        CheckAndUp(atUp, gameObject.GetComponent<Attributes>().name);
        CheckAndUp(atDn, gameObject.GetComponent<Attributes>().name);

    }

    //in event one node is broken into two or more nodes
    public void UpdateNodeBreak()
    {

        //objects for up down left right
        GameObject[] rog = Rogers();
        GameObject atUp, atDn, atLf, atRt;

        atLf = rog[0];
        atRt = rog[1];
        atUp = rog[2];
        atDn = rog[3];

        //updates adjacent wires with node name
        gameObject.GetComponent<ComponentUpdate>().done = true;

        //updates adjacent wires with node name
        CheckAndUp(atLf);
        CheckAndUp(atRt);
        CheckAndUp(atUp);
        CheckAndUp(atDn);

    }

    //components get prev and next and calls special namer for node attached to VS neg
    public void UpdateComponent(Transform s)
    {

        //get orientation
        Attributes.Orient dir = s.gameObject.GetComponent<Attributes>().orient;

        //objects for up down left right
        GameObject[] rog = Rogers();
        GameObject atUp, atDn, atLf, atRt;

        atLf = rog[0];
        atRt = rog[1];
        atUp = rog[2];
        atDn = rog[3];

        //adjust to front, back
        if (dir == Attributes.Orient.right)
        {

            s.gameObject.GetComponent<Attributes>().front = atRt;
            if(atRt && (s.gameObject.GetComponent<Attributes>().type != Attributes.Type.vSource))
                s.gameObject.GetComponent<Attributes>().voltagePlus = atRt.GetComponent<Attributes>().voltageMinus;
            s.gameObject.GetComponent<Attributes>().back = atLf;
            if (atLf && (s.gameObject.GetComponent<Attributes>().type != Attributes.Type.vSource))
                s.gameObject.GetComponent<Attributes>().voltageMinus = atLf.GetComponent<Attributes>().voltagePlus;

        }
        else if (dir == Attributes.Orient.up)
        {

            s.gameObject.GetComponent<Attributes>().front = atUp;
            if (atUp && (s.gameObject.GetComponent<Attributes>().type != Attributes.Type.vSource))
                s.gameObject.GetComponent<Attributes>().voltagePlus = atUp.GetComponent<Attributes>().voltageMinus;
            s.gameObject.GetComponent<Attributes>().back = atDn;
            if (atDn && (s.gameObject.GetComponent<Attributes>().type != Attributes.Type.vSource))
                s.gameObject.GetComponent<Attributes>().voltageMinus = atDn.GetComponent<Attributes>().voltagePlus;

        }
        else if (dir == Attributes.Orient.left)
        {

            s.gameObject.GetComponent<Attributes>().front = atLf;
            if (atLf && (s.gameObject.GetComponent<Attributes>().type != Attributes.Type.vSource))
                s.gameObject.GetComponent<Attributes>().voltagePlus = atLf.GetComponent<Attributes>().voltageMinus;
            s.gameObject.GetComponent<Attributes>().back = atRt;
            if (atRt && (s.gameObject.GetComponent<Attributes>().type != Attributes.Type.vSource))
                s.gameObject.GetComponent<Attributes>().voltageMinus = atRt.GetComponent<Attributes>().voltagePlus;

        }
        else if (dir == Attributes.Orient.down)
        {

            s.gameObject.GetComponent<Attributes>().front = atDn;
            if (atDn && (s.gameObject.GetComponent<Attributes>().type != Attributes.Type.vSource))
                s.gameObject.GetComponent<Attributes>().voltagePlus = atDn.GetComponent<Attributes>().voltageMinus;
            s.gameObject.GetComponent<Attributes>().back = atUp;
            if (atUp && (s.gameObject.GetComponent<Attributes>().type != Attributes.Type.vSource))
                s.gameObject.GetComponent<Attributes>().voltageMinus = atUp.GetComponent<Attributes>().voltagePlus;

        }
        else
            print("WARNING: Some failure has occured updating component");

        //Later on, this would be the place to add a function to delete another component placed next to the first with no wire in between.

    }

    //handles the special case of the grounded node attached to the negative of the vSource
    public void UpdateZeroNode()
    {
        
        gameObject.GetComponent<Attributes>().name = "n00";

        //objects for up down left right
        GameObject[] rog = Rogers();
        GameObject atUp, atDn, atLf, atRt;

        atLf = rog[0];
        atRt = rog[1];
        atUp = rog[2];
        atDn = rog[3];

        //updates adjacent wires with node name
        CheckAndUp(atLf, true);
        CheckAndUp(atRt, true);
        CheckAndUp(atUp, true);
        CheckAndUp(atDn, true);

    }

    //this function needs to iterate through a node. Each wire-wire contact is listed as an edge. 
    //Each wire-component contact checked to be either a source or a sink.
    //A list object should be passed by reference, so i can just pass it around as they call each other and fill the same object
    //I think I should have a new flag to identify when these are checked already. And the list object needs a variable to
    //identify sources and sinks. I'll probably need other variables that the shortest path algo will need to run and to return the results
    public void Edgy(List<GameObject> listiclese)
    {

        //objects for up down left right
        GameObject[] rog = Rogers();
        GameObject atLf, atRt, atUp, atDn;

        atLf = rog[0];
        atRt = rog[1];
        atUp = rog[2];
        atDn = rog[3];

        gameObject.GetComponent<ComponentUpdate>().edgeDone = true;
        listiclese.Add(gameObject);

        EdgeSet(listiclese, atLf);
        EdgeSet(listiclese, atRt);
        EdgeSet(listiclese, atUp);
        EdgeSet(listiclese, atDn);

    }

    //This function resets the vertex variables
    public void ResetVertex()
    {
        
        p = null;
        Adj = new List<Edge>();
        c = 0;
        d = 0;

    }

    //this helper function is going to decide what sort of edge to add to the edge list
    private void EdgeSet(List<GameObject> V, GameObject at)
    {

        //is this really a thing?
        if (at)
        {

            Adj.Add(new Edge(at, 0, Double.MaxValue, 'e'));

            //only run if the other node is unvisited
            if (!(at.GetComponent<ComponentUpdate>().edgeDone))
            {

                //determine what the component is
                //A wire recursively calls Edgy() to continue tracing the node
                if (at.GetComponent<Attributes>().type == Attributes.Type.wire)
                {
                    
                    Adj.Add(new Edge(at, 0, Double.MaxValue, 'e'));

                    at.GetComponent<ComponentUpdate>().Edgy(V);

                }
                //a component looks at options for which way the current is going
                else if (at.GetComponent<Attributes>().type == Attributes.Type.bResistor ||
                    at.GetComponent<Attributes>().type == Attributes.Type.mResistor ||
                    at.GetComponent<Attributes>().type == Attributes.Type.lResistor ||
                    at.GetComponent<Attributes>().type == Attributes.Type.vSource)
                {

                    V.Add(at);

                    if (at.GetComponent<Attributes>().front == gameObject)
                    {

                        if (at.GetComponent<Attributes>().amperage > 0)
                        {

                            //facing node with pos current
                            //this is a sink 't'
                            at.GetComponent<ComponentUpdate>().Adj.Add(new Edge(gameObject, 0, 0, 't'));
                            Adj.Add(new Edge(at, 0, at.GetComponent<Attributes>().amperage, 't'));
                            
                            dumbSink.GetComponent<ComponentUpdate>().Adj.Add(new Edge(at, 0, 0, 't'));
                            at.GetComponent<ComponentUpdate>().Adj.Add(new Edge(dumbSink, 0, Double.MaxValue, 't'));

                        }
                        else if (at.GetComponent<Attributes>().amperage < 0)
                        {

                            //facing node with neg current
                            //this is a source 's'
                            at.GetComponent<ComponentUpdate>().Adj.Add(new Edge(gameObject, 0, -at.GetComponent<Attributes>().amperage, 's'));
                            Adj.Add(new Edge(at, 0, 0, 's'));
                            
                            dumbSource.GetComponent<ComponentUpdate>().Adj.Add(new Edge(at, 0, Double.MaxValue, 's'));
                            at.GetComponent<ComponentUpdate>().Adj.Add(new Edge(dumbSource, 0, 0, 's'));

                        }
                        //when no current is passing through a component, it is ignored as a dead end
                        else
                        {

                            //no current

                        }

                    }
                    else if (at.GetComponent<Attributes>().back == gameObject)
                    {

                        if (at.GetComponent<Attributes>().amperage > 0)
                        {

                            //facing away from node with pos current
                            //this is a source 's'
                            at.GetComponent<ComponentUpdate>().Adj.Add(new Edge(gameObject, 0, at.GetComponent<Attributes>().amperage, 's'));
                            Adj.Add(new Edge(at, 0, 0, 's'));

                            dumbSource.GetComponent<ComponentUpdate>().Adj.Add(new Edge(at, 0, Double.MaxValue, 's'));
                            at.GetComponent<ComponentUpdate>().Adj.Add(new Edge(dumbSource, 0, 0, 's'));

                        }
                        else if (at.GetComponent<Attributes>().amperage < 0)
                        {

                            //facing away from node with neg current
                            //this is a sink 't'
                            at.GetComponent<ComponentUpdate>().Adj.Add(new Edge(gameObject, 0, 0, 't'));
                            Adj.Add(new Edge(at, 0, -at.GetComponent<Attributes>().amperage, 't'));
                            
                            dumbSink.GetComponent<ComponentUpdate>().Adj.Add(new Edge(at, 0, 0, 't'));
                            at.GetComponent<ComponentUpdate>().Adj.Add(new Edge(dumbSink, 0, Double.MaxValue, 't'));

                        }
                        //when no current is passing through a component, it is ignored as a dead end
                        else
                        {

                            //no current

                        }

                    }
                    else
                    {

                        print("WARNING: This wire's neighbor component is not assosciated with it.");

                    }

                }
                else
                {

                    print("WARNING: Adjacent object is of undefined type.");

                }

            }

        }

    }

    //This overloaded helper function returns the object in the direction specified by the shift and the column parameter
    GameObject GetDirectional(Transform p, String shift)
    {

        GameObject at = null;

        if (p.parent.Find(shift))
            if (p.parent.Find(shift).GetComponentsInChildren<Transform>().Length > 1)
                at = p.parent.Find(shift).GetComponentsInChildren<Transform>()[1].gameObject;

        return at;

    }
    GameObject GetDirectional(Transform p, String shift, string column)
    {

        GameObject at = null;
        
        if (p.parent.parent.Find(shift))
            if (p.parent.parent.Find(shift).Find(column).GetComponentsInChildren<Transform>().Length > 1)
                at = p.parent.parent.Find(shift).Find(column).GetComponentsInChildren<Transform>()[1].gameObject;

        return at;

    }

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

    //this overloaded helper function updates nodes with a given name or a new name or the zero name
    void CheckAndUp(GameObject at, String nameString)
    {

        if (at)
            if (!at.GetComponent<ComponentUpdate>().done)
                if (at.GetComponent<Attributes>().type == Attributes.Type.wire)
                    at.GetComponent<ComponentUpdate>().UpdateNode(nameString);

    }
    void CheckAndUp(GameObject at, bool zeroFlag)
    {

        if (at)
            if (!(at.GetComponent<Attributes>().name == "n00"))
                if (at.GetComponent<Attributes>().type == Attributes.Type.wire)
                    at.GetComponent<ComponentUpdate>().UpdateZeroNode();

    }
    void CheckAndUp(GameObject at)
    {

        Transform t = SearchUp("Foundation (UnityEngine.RectTransform)");

        if (at)
            if (!at.GetComponent<ComponentUpdate>().done)
                if (at.GetComponent<Attributes>().type == Attributes.Type.wire)
                    at.GetComponent<ComponentUpdate>().UpdateNode(t.GetComponent<SceneAttributes>().GetNodeName());

    }

    GameObject[] Rogers()
    {

        //This just cleans things up a bit
        Transform p = transform.parent;

        //get names as strings
        String colStr = p.ToString().Split(' ')[0];
        String rowStr = p.parent.ToString().Split(' ')[0];

        //get names as ints
        int col;
        int.TryParse(colStr, out col);
        int row;
        int.TryParse(rowStr, out row);

        //build names for adjacents
        String left = (col - 1).ToString();
        String right = (col + 1).ToString();
        String up = (row - 1).ToString();
        String down = (row + 1).ToString();

        //gets the object adjacent to this object if it exists
        GameObject[] objArray = { GetDirectional(p, left), GetDirectional(p, right), GetDirectional(p, up, colStr), GetDirectional(p, down, colStr) };

        return objArray;

    }

    private void Update()
    {

        done = false;

    }

    private void Start()
    {
        
        //sets the dummies for use by the path finder
        Transform t = SearchUp("Panel (UnityEngine.RectTransform)");
        dumbSink = t.Find("DummySink").gameObject;
        dumbSource = t.Find("DummySource").gameObject;

    }

}
