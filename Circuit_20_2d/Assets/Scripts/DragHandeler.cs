//all the crap we need
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

//drag handler has several responsibilties: as dragging and dropping pieces is the central action of the game,
//doing so invokes many other functions that return the consequences of those actions.
//The foundation of this handler is based on code found online. All the specific functionalities were written by our team
public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    //this is what we're dragging
	public static GameObject itemBeingDragged;

    //track where it came from to return it if the drag fails
	Vector3 startPosition;

    //track who it came from in case it needs to return. Also used in dragUpdate()
    Transform startParent;

    //These are used to update the wire shape based on orientation.
    public Sprite LWire;
    public Sprite TWire;
    public Sprite IWire;
    public Sprite XWire;

    //first part of drag process
    #region IBeginDragHandler implementation

    public void OnBeginDrag (PointerEventData eventData)
	{

        //this must be called here because it reflects an event that occurs when removing a piece breaks a node
        //if called later, it would be much more difficult to identify the nodes broken by the move.
        gameObject.GetComponent<ComponentUpdate>().UpdateNodeBreak();

        //drag process
        itemBeingDragged = gameObject;
		startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

	#endregion

    //second part of drag process
	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
    }

	#endregion

    //last part of drag process
	#region IEndDragHandler implementation
        
	public void OnEndDrag (PointerEventData eventData)
	{

        //Runs most of the events caused by a drag event
        DragUpdate(startParent);

        //finish drag sequence
		itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }

    }

    #endregion

    //calls update orientation for neighbors
    public void DragUpdate(Transform p)
    {

        //objects for up down left right
        GameObject[] rog = Rogers(p);
        GameObject atUp, atDn, atLf, atRt;

        atLf = rog[0];
        atRt = rog[1];
        atUp = rog[2];
        atDn = rog[3];

        //updates orientation of game object and the objects left behind
        gameObject.GetComponent<ComponentUpdate>().UpdateOrientation(true);
        if (atLf)
            atLf.GetComponent<ComponentUpdate>().UpdateOrientation(false);
        if (atRt)
            atRt.GetComponent<ComponentUpdate>().UpdateOrientation(false);
        if (atUp)
            atUp.GetComponent<ComponentUpdate>().UpdateOrientation(false);
        if (atDn)
            atDn.GetComponent<ComponentUpdate>().UpdateOrientation(false);


        //creates and applies names to components or begins node building
        if (gameObject.GetComponent<Attributes>().type == Attributes.Type.wire)
            gameObject.GetComponent<ComponentUpdate>().UpdateNode();

        else if (gameObject.GetComponent<Attributes>().type == Attributes.Type.bResistor ||
            gameObject.GetComponent<Attributes>().type == Attributes.Type.mResistor ||
            gameObject.GetComponent<Attributes>().type == Attributes.Type.lResistor)
            gameObject.GetComponent<Attributes>().name = "r_" + gameObject.GetComponent<Attributes>().row + "_" + gameObject.GetComponent<Attributes>().col;
        else if (gameObject.GetComponent<Attributes>().type == Attributes.Type.aMeter)
            gameObject.GetComponent<Attributes>().name = "A_" + gameObject.GetComponent<Attributes>().row + "_" + gameObject.GetComponent<Attributes>().col;
        else if (gameObject.GetComponent<Attributes>().type == Attributes.Type.vMeter)
            gameObject.GetComponent<Attributes>().name = "M_" + gameObject.GetComponent<Attributes>().row + "_" + gameObject.GetComponent<Attributes>().col;
        else if (gameObject.GetComponent<Attributes>().type == Attributes.Type.vSource)
            gameObject.GetComponent<Attributes>().name = "v_" + gameObject.GetComponent<Attributes>().row + "_" + gameObject.GetComponent<Attributes>().col;

        //gets transform for panel 
        Transform t = SearchUp("Panel (UnityEngine.RectTransform)");

        //begins the grid iteration
        t.Find("GamePanel").GetComponent<GameToAlgo>().IterateGrid();

        //refill resource panel children
        t.Find("ResourcePanel").GetComponent<ResourcePanelBuilder>().RefillChildren();

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

    GameObject[] Rogers(Transform p)
    {

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

}
