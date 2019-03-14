using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

// handles picking up and dropping gun
public class SwitchGun : MonoBehaviour {

    public SteamVR_Action_Boolean switchGunAction;

    Hand hand;
    GameObject gunObj;
    Gun gunScript;

    [System.NonSerialized]
    public bool overGun;

    //public ClipLogic clip;

    //public GameObject newClipPrefab;

    //public BoxCollider pickUpPrecision;

    //public bool buttonDown;

    //[System.NonSerialized]
    //public GameObject grabbedClip;

    private void OnEnable()
    {
        if (hand == null)
            hand = this.GetComponent<Hand>();

        if (switchGunAction == null)
        {
            Debug.LogError("<b>[SteamVR Interaction]</b> No grabClip action assigned");
            return;
        }

        switchGunAction.AddOnChangeListener(OnGripActionChange, hand.handType);

    }

    private void OnDisable()
    {
        if (switchGunAction != null)
            switchGunAction.RemoveOnChangeListener(OnGripActionChange, hand.handType);
    }

    private void OnGripActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
    {
        gunScript = hand.GetComponentInChildren<Gun>();

        // grip up and has gun
        if (! newValue && null != gunScript)
        {
            gunScript.DropGun();
            gunScript = null;
        }

        // if over new gun
        if(newValue && null == gunScript)
        {
            // run collision check

        }
    }
}
