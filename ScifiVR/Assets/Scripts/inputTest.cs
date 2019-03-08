using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using VRTK;

public class inputTest : MonoBehaviour {

    public GameObject dominantHand;
    public GameObject target;
    public LineRenderer trasportationRay;

    Color orig;
    float teleportSubLength;

	// Use this for initialization
	void Start () {
        orig = target.GetComponent<Renderer>().material.color;
        teleportSubLength = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
        if (dominantHand.GetComponent<VRTK_ControllerEvents>().IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TriggerPress))
        {
            target.SetActive(false);
            Debug.Log("touch");
        }
        else { target.SetActive(true); }

        if (dominantHand.GetComponent<VRTK_ControllerEvents>().IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TouchpadPress))
        {
            target.GetComponent<Renderer>().material.color = Color.blue;
            Debug.Log("trackpad");


        }
        else { target.GetComponent<Renderer>().material.color = orig; }
    }

    void DrawTeleportationRay()
    {
        float startRot = 0f;
        float currentRot = startRot;

    }
}
