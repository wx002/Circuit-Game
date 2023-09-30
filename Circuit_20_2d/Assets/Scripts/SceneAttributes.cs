using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAttributes : MonoBehaviour {

    public double bigR;
    public double medR;
    public double lilR;
    public double voltS;
    public double ampS;
    public double voltMP;
    public double voltMM;
    public double ampM;
    public string filePath;
    private int nodeName;

    //builds and returns a node name
    public string GetNodeName()
    {

        //name format "n##"
        string ret = "n" + nodeName.ToString("00");

        //limit of 99 names. 00 is reserved
        if (nodeName == 99)
            nodeName = 1;
        else
            nodeName++;

        return ret;

    }

    // Use this for initialization
    void Start () {

        string dp = Application.dataPath;
        filePath = dp + "/log/output.log";

        nodeName = 1;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
