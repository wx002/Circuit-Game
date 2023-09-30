using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class SPICEController
{
    private static readonly string SPICE_path = Application.dataPath + "/SPICE/ngspice_con.exe";
    public static readonly string log_path = Application.dataPath + "/log/output.log ";
    public static readonly string netlist_path = Application.dataPath + "/netlist/";
    private static readonly string SPICE_FLAGS = " -b -o ";

    

    /* test case for netllist
     * format:
     *      Type: List
     *      Element: tuples of size 4
     */

    public static List<string> netlist_test 
        = new List<string>();

	[Obsolete("Test function, use for basic testing ONLY")]
    public static void initTest()
    {
        //V_1_0 N_0_0 0 10
        netlist_test.Add("v_3_6 n14 n00 12");
        //R_1_1 N_0_1 N_2_1
        netlist_test.Add("r_3_8 n14 n00 10000");
    }

	[Obsolete("Aysnc version of call_SPICE, this should never be used unless SPICE is stalling the entire program")]
	public static void call_SPICE_async(string netlist)
    {
        //init process builder
        Process p = new Process();
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;


        string netpath = netlist_path + netlist;
        //setup command strings for SPICE
        string argv = SPICE_FLAGS + log_path + netpath;
        p.StartInfo.FileName = SPICE_path;
        p.StartInfo.Arguments = argv;

        //call SPICE
        Task.Run(() => p.Start()).Wait();
        //p.WaitForExit();
    }

    public static void call_SPICE(string netlist)
    {
        //init process builder
        Process p = new Process();
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;


        string netpath = netlist_path + netlist;
        //setup command strings for SPICE
        string argv = SPICE_FLAGS + log_path + netpath;
        p.StartInfo.FileName = SPICE_path;
        p.StartInfo.Arguments = argv;

        //UnityEngine.Debug.Log(SPICE_path);
        //UnityEngine.Debug.Log(netpath);
        //UnityEngine.Debug.Log(argv);

        //call SPICE
        p.Start();
        p.WaitForExit();
    }

    public static void generate_netlist(List<string> adjacency, string analysis)
    {
        FileStream fs = File.Create(netlist_path + "/netlist.cir");
        StreamWriter write = new StreamWriter(fs);
        write.WriteLine(";");//first line is name of circuit, leave it blank


        /*
         * TODO: loop thru given adt from GUI to build netlist based on positions
         * assuming grounded circuit for OP analysis
         */
         
        foreach (string s in adjacency)
        {
         
            if (s.Contains("n00"))
            {
                write.WriteLine(s.Replace("n00", "0") + " ; ");
            }
            else
            {
                write.WriteLine(s + " ; ");
            }
        }

        //ending

        //setup ending para
        string an = analysis + " ; ";
        write.WriteLine(an);
        write.WriteLine(".END ; ");
        write.Close();
        fs.Close();
    }



}

