﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ButtonFix : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        var watch = GameObject.Find("Watch");

        HoverButton wh = GetComponent<HoverButton>();
        GetComponent<HoverButton>().onButtonDown.AddListener(delegate { GetComponent<Valve.VR.InteractionSystem.Sample.InteractibleButton>().OnButtonDown(GameObject.Find("LeftHand").GetComponent<Hand>()); });
        GetComponent<HoverButton>().onButtonDown.AddListener(delegate { GetComponent<Valve.VR.InteractionSystem.Sample.InteractibleButton>().OnButtonDown(GameObject.Find("RightHand").GetComponent<Hand>()); });
        GetComponent<HoverButton>().onButtonDown.AddListener(delegate { GetComponent<AI_TalkTrigger>().Trigger(); });

        GetComponent<AI_TalkTrigger>().ToEmit = watch;
    }
}
