using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR.InteractionSystem.Sample;
using Valve.VR.InteractionSystem;

// give all buttons a reference to the player

public class InitButtonListeners : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // InteractibleButton[] buttons = FindObjectsOfType<InteractibleButton>();
        GameObject goal = GameObject.Find("TechTable_Animation");
        if(null == goal) { return; }
        InteractibleButton[] buttons = goal.GetComponentsInChildren<InteractibleButton>();
        HoverButton hb;
        GameObject lh = GameObject.Find("LeftHand");
        GameObject rh = GameObject.Find("RightHand");

        foreach (InteractibleButton b in buttons)
        {
            hb = b.GetComponent<HoverButton>();

            hb.onButtonDown.AddListener(delegate { b.OnButtonDown(lh.GetComponent<Hand>()); });
            hb.onButtonDown.AddListener(delegate { b.OnButtonDown(rh.GetComponent<Hand>()); });
            hb.onButtonUp.AddListener(delegate { b.OnButtonUp(lh.GetComponent<Hand>()); });
            hb.onButtonUp.AddListener(delegate { b.OnButtonUp(rh.GetComponent<Hand>()); });
            hb.onButtonIsPressed.AddListener(delegate { b.OnButtonPressed(lh.GetComponent<Hand>()); });
            hb.onButtonIsPressed.AddListener(delegate { b.OnButtonPressed(rh.GetComponent<Hand>()); });

            hb.onButtonDown.AddListener(delegate { GetComponent<AI_TalkTrigger>().Trigger(); });          
        }
	}
	
}
