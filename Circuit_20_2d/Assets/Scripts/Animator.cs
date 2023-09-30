using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animator : MonoBehaviour {

    private string fulllog;
    private List<List<string>> parsed;
    public Dictionary<string, double> current_dic;
    public Dictionary<string, double> voltage_dic;

    //I don't know how animation works yet, so I'm putting the code I know I'll need in here for now. Honestly
    public void Tesla(GameObject[][] grid, int rowCount, int colCount, double voltMax)
    {

        for (int i = 0; i < rowCount; i++)
            for (int j = 0; j < colCount; j++)
                if (grid[i][j])
                {

                    double vP = grid[i][j].GetComponent<Attributes>().voltagePlus;
                    double vM = grid[i][j].GetComponent<Attributes>().voltageMinus;

                    float pCol = (float)((vP / voltMax));
                    float mCol = (float)((vM / voltMax));

                    float portRed = grid[i][j].GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color.r;
                    float portGreen = grid[i][j].GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color.g;
                    float portBlue = grid[i][j].GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color.b;
                    float portAlpha = grid[i][j].GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color.a;

                    float starboardRed = grid[i][j].GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color.r;
                    float starboardGreen = grid[i][j].GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color.g;
                    float starboardBlue = grid[i][j].GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color.b;
                    float starboardAlpha = grid[i][j].GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color.a;

                    float forwardRed = grid[i][j].GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color.r;
                    float forwardGreen = grid[i][j].GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color.g;
                    float forwardBlue = grid[i][j].GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color.b;
                    float forwardAlpha = grid[i][j].GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color.a;

                    float backwardRed = grid[i][j].GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color.r;
                    float backwardGreen = grid[i][j].GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color.g;
                    float backwardBlue = grid[i][j].GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color.b;
                    float backwardAlpha = grid[i][j].GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color.a;

                    grid[i][j].GetComponentInChildren<Transform>().Find("voltPort").gameObject.GetComponent<Image>().color = new Color(pCol, portGreen, portBlue, portAlpha);
                    grid[i][j].GetComponentInChildren<Transform>().Find("voltStarboard").gameObject.GetComponent<Image>().color = new Color(pCol, starboardGreen, starboardBlue, starboardAlpha);
                    grid[i][j].GetComponentInChildren<Transform>().Find("voltForward").gameObject.GetComponent<Image>().color = new Color(pCol, forwardGreen, forwardBlue, forwardAlpha);
                    grid[i][j].GetComponentInChildren<Transform>().Find("voltBackward").gameObject.GetComponent<Image>().color = new Color(mCol, backwardGreen, backwardBlue, backwardAlpha);

                }

    }

	// Use this for initialization
	void Start ()
    {

        //fulllog = Parser.read_file("");
        //parsed = Parser.get_reduced_list(Parser.parse_text(fulllog));

        //current_dic = Parser.get_component_data(parsed, Parser.DataType.CURRENT);
        //voltage_dic = Parser.get_voltage(parsed);

        /*
         * loop thru all components
         * check if contain in dictionary using dic.Contains(key)
         * update attributes if it does contain keys
         * access by dic["key"]
         */
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
