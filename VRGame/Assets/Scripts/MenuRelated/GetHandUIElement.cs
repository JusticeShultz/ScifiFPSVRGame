using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHandUIElement : Valve.VR.InteractionSystem.UIElement {

	public GameObject GetCurrentHand()
    {
        return currentHand.gameObject;
    }

    public void SendCurrentHand()
    {
        //GetComponent<HeightSlider>().currentHand = currentHand.gameObject;
        //GetComponent<HeightSlider>().handScript = currentHand;
        //GetComponent<HeightSlider>().anchoring = true;
        print("sent");
    }
}
