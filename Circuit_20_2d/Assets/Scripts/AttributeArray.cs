using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeArray : MonoBehaviour {

    //get array sizes
    static int r = 4; //these need to actually be determined
    static int c = 12; //these need to actually be determined

    //struct for attributes data
    public struct Attrib
    {

        public Attributes.Type type;
        public Attributes.Orient orient;
        public Attributes.Direct direct;

        public double resistance;
        public double amperage;
        public double voltagePlus;
        public double voltageMinus;
        public int row;
        public int col;
        public int strength;

    }

    //create Array
    Attrib[,] attribArray = new Attrib[r, c];

    //build game field
    public void FieldFill()
    {

        GameObject fieldOfPlay = GameObject.Find("GamePanel");

        for (int i = 0; i < r; i++)
        {

            for (int j = 0; i < c; j++)
            {

            }

        }

    }

    //fill array
    public void ArrayFill()
    {

        GameObject fieldOfPlay = GameObject.Find("GamePanel");

        for (int i = 0; i < r; i++)
        {

            Transform rowObj = fieldOfPlay.transform.GetChild(i);

            for (int j = 0; i < c; j++)
            {

                Transform colObj = rowObj.transform.GetChild(j);

                if (colObj.gameObject.GetComponentInChildren<Attributes>())
                {

                    attribArray[i, j].type = colObj.gameObject.GetComponentInChildren<Attributes>().type;
                    attribArray[i, j].orient = colObj.gameObject.GetComponentInChildren<Attributes>().orient;
                    attribArray[i, j].direct = colObj.gameObject.GetComponentInChildren<Attributes>().direct;
                    attribArray[i, j].resistance = colObj.gameObject.GetComponentInChildren<Attributes>().resistance;
                    attribArray[i, j].amperage = colObj.gameObject.GetComponentInChildren<Attributes>().amperage;
                    attribArray[i, j].voltagePlus = colObj.gameObject.GetComponentInChildren<Attributes>().voltagePlus;
                    attribArray[i, j].voltageMinus = colObj.gameObject.GetComponentInChildren<Attributes>().voltageMinus;
                    attribArray[i, j].row = colObj.gameObject.GetComponentInChildren<Attributes>().row;
                    attribArray[i, j].col = colObj.gameObject.GetComponentInChildren<Attributes>().col;
                    attribArray[i, j].strength = colObj.gameObject.GetComponentInChildren<Attributes>().strength;

                }

            }

        }

    }

    //update change in array
    void ArrayUpdate()
    {

        for (int i = 0; i < r; i++)
        {

            for (int j = 0; i < c; j++)
            {

                //update the cell
                //might just look like fill...

            }

        }

    }
}
