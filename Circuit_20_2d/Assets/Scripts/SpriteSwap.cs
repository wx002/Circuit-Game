using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpriteSwap : MonoBehaviour
{

    //sprite repository
    Image myImageComponent;
    public Sprite pipeNipple; 
    public Sprite pipeElbow; 
    public Sprite pipeTea; 
    public Sprite pipeCross;
    public Sprite sourceVolt;
    public Sprite sourceAmp; 
    public Sprite meterVolt; 
    public Sprite meterAmp; 
    public Sprite resistorSmall;
    public Sprite resistorMedium;
    public Sprite resistorLarge; 

    // I forget what this is for
    void Start()
    {

        myImageComponent = GetComponent<Image>();

    }

}
