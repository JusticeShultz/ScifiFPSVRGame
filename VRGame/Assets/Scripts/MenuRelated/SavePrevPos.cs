using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;
using Valve.VR.InteractionSystem;

public class SavePrevPos : MonoBehaviour {

    SteamVR_Input_Sources leftHand;
    SteamVR_Input_Sources rightHand;
    bool wasUp;
    bool currentUp;

    public static Vector3 playerPrevPos;

    void Start()
    {
        leftHand = GameObject.Find("LeftHand").GetComponent<Hand>().handType;
        rightHand = GameObject.Find("RightHand").GetComponent<Hand>().handType;
        playerPrevPos = transform.position;

        wasUp = true;
        currentUp = true;
    }
    void Update () {
        currentUp = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport").GetStateUp(leftHand) || SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport").GetStateUp(rightHand);
        if (!currentUp && wasUp)
        {
            playerPrevPos = transform.position;
        }
        wasUp = currentUp;
	}
}
