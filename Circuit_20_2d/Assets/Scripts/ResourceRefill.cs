using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceRefill : MonoBehaviour
{

    public GameObject itemPrefab;

    Attributes.Type setType = new Attributes.Type();

    Image setImage;

    double setDouble;

    public void Reload()
    {

        //print("Reloding...");
        string searchString = transform.ToString();

        //There should be an if statement here to establish if this slot has any children, and this method should be called on any drag event.
        //Everything gets skipped if appropriate, otherwise, the slot figures out what it needs and fills itself.
        if (gameObject.transform.childCount == 0)
        {
            
            //what is the slot supposed to hold?
            if (string.Equals(searchString, "vsBack (UnityEngine.RectTransform)"))
            {

                GameObject newObj = Instantiate(itemPrefab) as GameObject;
                newObj.transform.SetParent(gameObject.transform);

                //print("Attempting to reload Voltage Source");

                //name it
                newObj.transform.name = "voltageSource";

                //Set type
                setType = Attributes.Type.vSource;
                newObj.GetComponent<Attributes>().type = setType;

                //Assign an image sprite
                setImage = newObj.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>();
                setImage.sprite = gameObject.GetComponent<Transform>().parent.gameObject.GetComponent<ResourcePanelBuilder>().vsSprite; 

                //Give it values.
                setDouble = (gameObject.transform.parent.parent.parent.GetComponent<SceneAttributes>().voltS / 2);
                newObj.GetComponent<Attributes>().voltagePlus = setDouble;
                setDouble = -(gameObject.transform.parent.parent.parent.GetComponent<SceneAttributes>().voltS / 2);
                newObj.GetComponent<Attributes>().voltageMinus = setDouble;

            }
            else if (string.Equals(searchString, "asBack (UnityEngine.RectTransform)"))
            {

                GameObject newObj = Instantiate(itemPrefab) as GameObject;
                newObj.transform.SetParent(gameObject.transform);

                //name it
                newObj.transform.name = "amperageSource";

                //Set type
                setType = Attributes.Type.aSource;
                newObj.GetComponent<Attributes>().type = setType;

                //Assign an image sprite
                //setImage = newObj.GetComponent<Image>();
                //setImage.sprite = gameObject.GetComponent<Transform>().parent.gameObject.GetComponent<ResourcePanelBuilder>().asSprite; 

                //Give it values.
                setDouble = (gameObject.transform.parent.parent.parent.GetComponent<SceneAttributes>().ampS);
                newObj.GetComponent<Attributes>().amperage = setDouble;

            }
            else if (string.Equals(searchString, "brBack (UnityEngine.RectTransform)"))
            {

                GameObject newObj = Instantiate(itemPrefab) as GameObject;
                newObj.transform.SetParent(gameObject.transform);

                //name it
                newObj.transform.name = "bigResistor";

                //Set type
                setType = Attributes.Type.bResistor;
                newObj.GetComponent<Attributes>().type = setType;

                //Assign an image sprite
                setImage = newObj.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>();
                setImage.sprite = gameObject.GetComponent<Transform>().parent.gameObject.GetComponent<ResourcePanelBuilder>().brSprite;

                //Give it values.
                setDouble = (gameObject.transform.parent.parent.parent.GetComponent<SceneAttributes>().bigR);
                newObj.GetComponent<Attributes>().resistance = setDouble;

            }
            else if (string.Equals(searchString, "mrBack (UnityEngine.RectTransform)"))
            {

                GameObject newObj = Instantiate(itemPrefab) as GameObject;
                newObj.transform.SetParent(gameObject.transform);

                //name it
                newObj.transform.name = "mediumResistor";

                //Set type
                setType = Attributes.Type.mResistor;
                newObj.GetComponent<Attributes>().type = setType;

                //Assign an image sprite
                setImage = newObj.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>();
                setImage.sprite = gameObject.GetComponent<Transform>().parent.gameObject.GetComponent<ResourcePanelBuilder>().mrSprite;

                //Give it values.
                setDouble = (gameObject.transform.parent.parent.parent.GetComponent<SceneAttributes>().medR);
                newObj.GetComponent<Attributes>().resistance = setDouble;

            }
            else if (string.Equals(searchString, "lrBack (UnityEngine.RectTransform)"))
            {

                GameObject newObj = Instantiate(itemPrefab) as GameObject;
                newObj.transform.SetParent(gameObject.transform);

                //name it
                newObj.transform.name = "littleResistor";

                //Set type
                setType = Attributes.Type.lResistor;
                newObj.GetComponent<Attributes>().type = setType;

                //Assign an image sprite
                setImage = newObj.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>();
                setImage.sprite = gameObject.GetComponent<Transform>().parent.gameObject.GetComponent<ResourcePanelBuilder>().lrSprite;

                //Give it values.
                setDouble = (gameObject.transform.parent.parent.parent.GetComponent<SceneAttributes>().lilR);
                newObj.GetComponent<Attributes>().resistance = setDouble;

            }
            else if (string.Equals(searchString, "vmBack (UnityEngine.RectTransform)"))
            {

                GameObject newObj = Instantiate(itemPrefab) as GameObject;
                newObj.transform.SetParent(gameObject.transform);

                //name it
                newObj.transform.name = "voltMeter";

                //Set type
                setType = Attributes.Type.vMeter;
                newObj.GetComponent<Attributes>().type = setType;

                //Assign an image sprite
                setImage = newObj.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>();
                setImage.sprite = gameObject.GetComponent<Transform>().parent.gameObject.GetComponent<ResourcePanelBuilder>().vmSprite;

                //Give it values.
                setDouble = (gameObject.transform.parent.parent.parent.GetComponent<SceneAttributes>().voltMP);
                newObj.GetComponent<Attributes>().voltagePlus = setDouble;
                setDouble = (gameObject.transform.parent.parent.parent.GetComponent<SceneAttributes>().voltMM);
                newObj.GetComponent<Attributes>().voltageMinus = setDouble;

            }
            else if (string.Equals(searchString, "amBack (UnityEngine.RectTransform)"))
            {

                GameObject newObj = Instantiate(itemPrefab) as GameObject;
                newObj.transform.SetParent(gameObject.transform);

                //name it
                newObj.transform.name = "ammeter";

                //Set type
                setType = Attributes.Type.aMeter;
                newObj.GetComponent<Attributes>().type = setType;

                //Assign an image sprite
                setImage = newObj.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>();
                setImage.sprite = gameObject.GetComponent<Transform>().parent.gameObject.GetComponent<ResourcePanelBuilder>().amSprite;

                //Give it values.
                setDouble = (gameObject.transform.parent.parent.parent.GetComponent<SceneAttributes>().ampM);
                newObj.GetComponent<Attributes>().amperage = setDouble;

            }
            else if (string.Equals(searchString, "wBack (UnityEngine.RectTransform)"))
            {

                GameObject newObj = Instantiate(itemPrefab) as GameObject;
                newObj.transform.SetParent(gameObject.transform);

                //name it
                newObj.transform.name = "wire";

                //Set type
                setType = Attributes.Type.wire;
                newObj.GetComponent<Attributes>().type = setType;

                //Assign an image sprite
                setImage = newObj.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>();
                setImage.sprite = gameObject.GetComponent<Transform>().parent.gameObject.GetComponent<ResourcePanelBuilder>().wSprite;

            }
            else
            {

                //error msg

            }

        }
        else if (gameObject.transform.childCount == 1 && string.Equals(searchString, "tBack (UnityEngine.RectTransform)"))
        {

            //print("made it to tBack");
            Destroy(transform.GetComponentsInChildren<Transform>()[1].gameObject);

        }

        

    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update () {
		
	}
}
