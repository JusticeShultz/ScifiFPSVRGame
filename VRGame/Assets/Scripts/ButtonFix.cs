using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ButtonFix : MonoBehaviour
{
    public bool IsAIButton = true;

	void Start ()
    {
        if(!IsAIButton)
        {
            HoverButton wh1 = GetComponent<HoverButton>();

            wh1.onButtonDown.AddListener(delegate
            {
                GetComponent<Valve.VR.InteractionSystem.Sample.InteractibleButton>().OnButtonDown(GameObject.Find("LeftHand").GetComponent<Hand>());
            });

            wh1.onButtonDown.AddListener(delegate
            {
                GetComponent<Valve.VR.InteractionSystem.Sample.InteractibleButton>().OnButtonDown(GameObject.Find("RightHand").GetComponent<Hand>());
            });

            wh1.onButtonIsPressed.AddListener(delegate
            {
                GetComponent<Valve.VR.InteractionSystem.Sample.InteractibleButton>().OnButtonPressed(GameObject.Find("LeftHand").GetComponent<Hand>());
            });

            wh1.onButtonIsPressed.AddListener(delegate
            {
                GetComponent<Valve.VR.InteractionSystem.Sample.InteractibleButton>().OnButtonPressed(GameObject.Find("RightHand").GetComponent<Hand>());
            });

            wh1.onButtonUp.AddListener(delegate
            {
                GetComponent<Valve.VR.InteractionSystem.Sample.InteractibleButton>().OnButtonUp(GameObject.Find("LeftHand").GetComponent<Hand>());
            });

            wh1.onButtonUp.AddListener(delegate
            {
                GetComponent<Valve.VR.InteractionSystem.Sample.InteractibleButton>().OnButtonUp(GameObject.Find("RightHand").GetComponent<Hand>());
            });

            return;
        }

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
