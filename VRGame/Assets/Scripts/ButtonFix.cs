using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ButtonFix : MonoBehaviour
{
	void Start ()
    {
        var watch = GameObject.Find("Watch");
        var aiArm = GameObject.Find("Player");
        var animation = GameObject.Find("VRCamera");

        HoverButton wh = GetComponent<HoverButton>();
        wh.onButtonDown.AddListener(delegate 
        {
            GetComponent<Valve.VR.InteractionSystem.Sample.InteractibleButton>().OnButtonDown(GameObject.Find("LeftHand").GetComponent<Hand>());
        });

        wh.onButtonDown.AddListener(delegate 
        {
            GetComponent<Valve.VR.InteractionSystem.Sample.InteractibleButton>().OnButtonDown(GameObject.Find("RightHand").GetComponent<Hand>());
        });

        wh.onButtonDown.AddListener(delegate 
        {
            GetComponent<AI_TalkTrigger>().Trigger();
        });

        wh.onButtonDown.AddListener(delegate 
        {
            aiArm.GetComponent<Reference>().referenceType.SetActive(true);
        });

        wh.onButtonDown.AddListener(delegate 
        { animation.GetComponent<Animator>().enabled = true;
        });

        GetComponent<AI_TalkTrigger>().ToEmit = watch;
    }
}
