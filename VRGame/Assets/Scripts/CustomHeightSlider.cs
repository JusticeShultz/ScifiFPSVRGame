using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CustomHeightSlider : MonoBehaviour
{
    public float Max = 2.0f;
    public float Min = -2.0f;
    public SteamVR_Action_Boolean slideAction;
    public bool IsHoveredByRight = false;
    public bool IsHoveredByLeft = false;
    public float Amount = 0.0f;
    public float LastSavedAmount = 0.0f;
    public SteamVR_Input_Sources LeftHand;
    public SteamVR_Input_Sources RightHand;
    public Valve.VR.Extras.SteamVR_LaserPointer LeftPointer;
    public Valve.VR.Extras.SteamVR_LaserPointer RightPointer;
    public Material Unhovered;
    public Material Hovered;

    private bool SafeHoldLeft = false;
    private bool SafeHoldRight = false;

    void Start ()
    {
        //Unserialize our data, if it never existed just default to 0.
        Amount = PlayerPrefs.GetFloat("Height", 0);
	}
	
	void Update ()
    {
        //Check if we are hovering this object with the left pointer.
        if(LeftPointer.hitObject != null)
            if (LeftPointer.hitObject == gameObject)
                IsHoveredByLeft = true;
            else IsHoveredByLeft = false;

        //Check if we are hovering this object with the right pointer.
        if (RightPointer.hitObject != null)
            if (RightPointer.hitObject == gameObject)
                IsHoveredByRight = true;
            else IsHoveredByRight = false;
        
        //Save our data only when necessary.
        if (Amount != LastSavedAmount)
        {
            PlayerPrefs.SetFloat("Height", Amount);
            LastSavedAmount = Amount;
        }

        //Set our sliders position to our amount value.
        transform.position = new Vector3(-2.3f, 1, Amount);
        
        //If we are hovering and holding on the left hand.
        if (IsHoveredByLeft && slideAction.GetState(LeftHand))
        {
            SafeHoldLeft = true;
            Amount = Mathf.Lerp(Amount, Mathf.Clamp(LeftPointer.hitPoint.z, Min, Max), 0.2f);
        }

        //If we are hovering and holding on the right hand.
        if (IsHoveredByRight && slideAction.GetState(RightHand))
        {
            SafeHoldRight = true;
            Amount = Mathf.Lerp(Amount, Mathf.Clamp(RightPointer.hitPoint.z, Min, Max), 0.2f);
        }

        if (slideAction.GetState(LeftHand) && SafeHoldLeft)
            Amount = Mathf.Lerp(Amount, Mathf.Clamp(LeftPointer.hitPoint.z, Min, Max), 0.2f);
        else if (!slideAction.GetState(LeftHand) && SafeHoldLeft) SafeHoldLeft = false;

        if (slideAction.GetState(RightHand) && SafeHoldRight)
            Amount = Mathf.Lerp(Amount, Mathf.Clamp(RightPointer.hitPoint.z, Min, Max), 0.2f);
        else if (!slideAction.GetState(RightHand) && SafeHoldRight) SafeHoldRight = false;

        if (!IsHoveredByLeft && !IsHoveredByRight)
            GetComponent<MeshRenderer>().material = Unhovered;
        else GetComponent<MeshRenderer>().material = Hovered;
    }
}
