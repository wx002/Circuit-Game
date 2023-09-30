using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanelBuilder : MonoBehaviour {

    public GameObject genPrefab;
    public GameObject vsPrefab;
    //public GameObject asPrefab;
    public GameObject brPrefab;
    public GameObject mrPrefab;
    public GameObject lrPrefab;
    public GameObject vmPrefab;
    public GameObject amPrefab;
    public GameObject wPrefab;

    public Sprite vsSprite;
    //public Sprite asSprite;
    public Sprite brSprite;
    public Sprite mrSprite;
    public Sprite lrSprite;
    public Sprite vmSprite;
    public Sprite amSprite;
    public Sprite wSprite;

    // Use this for initialization
    void Start ()
    {

        Attributes.Type setType = new Attributes.Type();
        Attributes.Orient setOrient = new Attributes.Orient();
        Attributes.Direct setDirect = new Attributes.Direct();

        Image setImage;
        
        double setDouble;
        int setRow;
        int setCol;
        bool setStrength;

        //voltage source slot and object
        GameObject vsbObj = Instantiate(genPrefab) as GameObject;
        vsbObj.transform.SetParent(gameObject.transform);
        vsbObj.transform.name = "vsBack";
        GameObject vsObj = Instantiate(vsPrefab) as GameObject;
        vsObj.transform.SetParent(vsbObj.transform);
        vsObj.transform.name = "voltageSource";

        setType = Attributes.Type.vSource;
        vsObj.GetComponent<Attributes>().type = setType;

        setImage = vsObj.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>();
        setImage.sprite = vsSprite;

        setDouble = (gameObject.transform.parent.parent.GetComponent<SceneAttributes>().voltS);
        vsObj.GetComponent<Attributes>().voltagePlus = setDouble;
        setDouble = -(0);
        vsObj.GetComponent<Attributes>().voltageMinus = setDouble;


        //amperage source slot and object
        /**
        GameObject asbObj = Instantiate(genPrefab) as GameObject;
        asbObj.transform.SetParent(gameObject.transform);
        asbObj.transform.name = "asBack";
        GameObject asObj = Instantiate(asPrefab) as GameObject;
        asObj.transform.SetParent(asbObj.transform);
        asObj.transform.name = "amperageSource";

        setType = Attributes.Type.aSource;
        asObj.GetComponent<Attributes>().type = setType;

        setImage = asObj.GetComponent<Image>();
        setImage.sprite = asSprite;

        setDouble = gameObject.transform.parent.parent.GetComponent<SceneAttributes>().ampS;
        asObj.GetComponent<Attributes>().amperage = setDouble;
        */

        //big resistor slot and object
        GameObject brbObj = Instantiate(genPrefab) as GameObject;
        brbObj.transform.SetParent(gameObject.transform);
        brbObj.transform.name = "brBack";
        GameObject brObj = Instantiate(brPrefab) as GameObject;
        brObj.transform.SetParent(brbObj.transform);
        brObj.transform.name = "bigResistor";

        setType = Attributes.Type.bResistor;
        brObj.GetComponent<Attributes>().type = setType;

        setImage = brObj.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>();
        setImage.sprite = brSprite;

        setDouble = gameObject.transform.parent.parent.GetComponent<SceneAttributes>().bigR;
        brObj.GetComponent<Attributes>().resistance = setDouble;

        //medium resistor slot and object
        GameObject mrbObj = Instantiate(genPrefab) as GameObject;
        mrbObj.transform.SetParent(gameObject.transform);
        mrbObj.transform.name = "mrBack";
        GameObject mrObj = Instantiate(mrPrefab) as GameObject;
        mrObj.transform.SetParent(mrbObj.transform);
        mrObj.transform.name = "mediumResistor";

        setType = Attributes.Type.mResistor;
        mrObj.GetComponent<Attributes>().type = setType;

        setImage = mrObj.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>();
        setImage.sprite = mrSprite;

        setDouble = gameObject.transform.parent.parent.GetComponent<SceneAttributes>().medR;
        mrObj.GetComponent<Attributes>().resistance = setDouble;

        //lil resistor slot and object
        GameObject lrbObj = Instantiate(genPrefab) as GameObject;
        lrbObj.transform.SetParent(gameObject.transform);
        lrbObj.transform.name = "lrBack";
        GameObject lrObj = Instantiate(lrPrefab) as GameObject;
        lrObj.transform.SetParent(lrbObj.transform);
        lrObj.transform.name = "littleResistor";

        setType = Attributes.Type.lResistor;
        lrObj.GetComponent<Attributes>().type = setType;

        setImage = lrObj.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>();
        setImage.sprite = lrSprite;

        setDouble = gameObject.transform.parent.parent.GetComponent<SceneAttributes>().lilR;
        lrObj.GetComponent<Attributes>().resistance = setDouble;

        //volt meter slot and object
        GameObject vmbObj = Instantiate(genPrefab) as GameObject;
        vmbObj.transform.SetParent(gameObject.transform);
        vmbObj.transform.name = "vmBack";
        GameObject vmObj = Instantiate(vmPrefab) as GameObject;
        vmObj.transform.SetParent(vmbObj.transform);
        vmObj.transform.name = "voltageMeter";

        setType = Attributes.Type.vMeter;
        vmObj.GetComponent<Attributes>().type = setType;



        setImage = vmObj.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>();
        setImage.sprite = vmSprite;

        setDouble = (gameObject.transform.parent.parent.GetComponent<SceneAttributes>().voltMP);
        vmObj.GetComponent<Attributes>().voltagePlus = setDouble;
        setDouble = -(gameObject.transform.parent.parent.GetComponent<SceneAttributes>().voltMM);
        vmObj.GetComponent<Attributes>().voltageMinus = setDouble;

        //ammeter slot and object
        GameObject ambObj = Instantiate(genPrefab) as GameObject;
        ambObj.transform.SetParent(gameObject.transform);
        ambObj.transform.name = "amBack";
        GameObject amObj = Instantiate(amPrefab) as GameObject;
        amObj.transform.SetParent(ambObj.transform);
        amObj.transform.name = "ammeter";

        setType = Attributes.Type.aMeter;
        amObj.GetComponent<Attributes>().type = setType;

        setImage = amObj.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>();
        setImage.sprite = amSprite;

        setDouble = gameObject.transform.parent.parent.GetComponent<SceneAttributes>().ampM;
        amObj.GetComponent<Attributes>().amperage = setDouble;

        //wire slot and object
        GameObject wbObj = Instantiate(genPrefab) as GameObject;
        wbObj.transform.SetParent(gameObject.transform);
        wbObj.transform.name = "wBack";
        GameObject wObj = Instantiate(wPrefab) as GameObject;
        wObj.transform.SetParent(wbObj.transform);
        wObj.transform.name = "wire";

        setType = Attributes.Type.wire;
        wObj.GetComponent<Attributes>().type = setType;

        setImage = wObj.GetComponentInChildren<Transform>().Find("Component").gameObject.GetComponent<Image>();
        setImage.sprite = wSprite;

        //trash slot and object
        GameObject tbObj = Instantiate(genPrefab) as GameObject;
        tbObj.transform.SetParent(gameObject.transform);
        tbObj.transform.name = "tBack";

        tbObj.GetComponent<Image>().color = new Color(255, 0, 0);
        tbObj.GetComponent<Image>().fillCenter = false;

    }

    public void RefillChildren()
    {

        foreach (Transform child in transform)
            child.gameObject.GetComponent<ResourceRefill>().Reload();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
