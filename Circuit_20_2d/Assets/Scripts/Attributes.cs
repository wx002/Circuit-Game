using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour {

    public string name;
    public enum Type { vSource, aSource, bResistor, mResistor, lResistor, vMeter, aMeter, wire};
    public enum Orient { up, down, left, right, };
    public enum Direct { up, down, left, right, };

    public Type type;
    public Orient orient;
    public int orientVal;
    public Direct direct;
    public int directVal;

    public double resistance;
    public double amperage;
    public double voltagePlus;
    public double voltageMinus;
    public int row;
    public int col;
    public int strength;

    public GameObject front;
    public GameObject back;

    //initialize strengths to -1
    private void Start()
    {
        gameObject.GetComponent<Attributes>().strength = -1;
    }

}
