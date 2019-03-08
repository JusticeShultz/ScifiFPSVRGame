using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class Shooting : MonoBehaviour
{
    public GameObject LeftHand;
    public GameObject RightHand;

    void Start()
    {
    }

    void Update()
    {
        var lefthand = LeftHand.GetComponent<SteamVR_Behaviour_Pose>();
        var righthand = RightHand.GetComponent<SteamVR_Behaviour_Pose>();

        // lefthand.
    }
}