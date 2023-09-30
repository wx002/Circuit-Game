using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanelBuilder : MonoBehaviour {

    //initialize game panel width and heigth
    public float gamePanelWidth = 0;
    public float gamePanelHeigth = 0;

    //slot width and heigth
    public int slotWidth = 45;
    public int slotHeigth = 45;

    //slot spacing
    public int slotSpacing = 5;

    //numRows
    public int numRows = 0;
    //numCols
    public int numCols = 0;

    //variables to hold prefabs
    public GameObject rowPrefab;
    public GameObject colPrefab;

    // Use this for initialization
    void Start () {

        //defines rectangleTransform object as go's grandparent's
        RectTransform rt = gameObject.transform.parent.parent.GetComponent<RectTransform>();

        //This gets the size of the original object, I believe
        gamePanelWidth = gameObject.GetComponent<RectTransform>().rect.xMax - gameObject.GetComponent<RectTransform>().rect.xMin;
        gamePanelHeigth = gameObject.GetComponent<RectTransform>().rect.yMax - gameObject.GetComponent<RectTransform>().rect.yMin;

        //Calculate number of rows and columns
        numRows = (int)(gamePanelHeigth / (slotHeigth + (2 * slotSpacing)));
        numCols = (int)(gamePanelWidth / (slotWidth + (2 * slotSpacing)));

        //create rows
        for (int i = 0; i < numRows; i++)
        {
            
            //instantiates rows, assign them, name them
            GameObject rowObj = Instantiate(rowPrefab) as GameObject;
            rowObj.transform.SetParent(gameObject.transform);
            rowObj.transform.name = i.ToString();

            //create columns
            for(int j = 0; j < numCols; j++)
            {
                
                //instantiate columns, assign them to parent row, name them
                GameObject colObj = Instantiate(colPrefab) as GameObject;
                colObj.transform.SetParent(rowObj.transform);
                colObj.transform.name = j.ToString();

            }

        }

    }

}
