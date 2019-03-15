using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;
using UnityEngine.VR;
using Valve.VR.InteractionSystem;


// 3 weapons - right, left, holster

public class WeaponHandler : MonoBehaviour {

    public GameObject leftWeapon;
    public GameObject rightWeapon;
    GameObject holsterWeapon;

    GameObject leftHand;
    GameObject rightHand;

    ClipLogic clipLogic;

	// Use this for initialization
	void Start ()
    {
        leftHand = GameObject.Find("LeftHand");
        rightHand = GameObject.Find("RightHand");
        // rightWeapon = rightHand.GetComponentInChildren<Gun>().gameObject;
        // leftWeapon = leftHand.GetComponentInChildren<Gun>().gameObject;
        clipLogic = GetComponentInChildren<ClipLogic>();
    }

    void PickupWeapon(GameObject hand, GameObject weapon)
    {
        weapon.GetComponent<Gun>().PickupGun(hand.transform.Find("HoverPoint"));
        weapon.GetComponent<Gun>().HandType = hand.GetComponent<Hand>().handType;
        if (hand == leftHand) { leftWeapon = weapon; }
        if (hand == rightHand) { rightWeapon = weapon; }
    }

    void DropWeapon(GameObject hand)
    {
        hand.GetComponentInChildren<Gun>().DropGun();
        if (hand == rightHand) { rightWeapon = null; }
        if (hand == leftHand) { leftWeapon = null; }
    }

    // called by interactable on grip down
    public void OnPickup(GameObject weapon)
    {
        GameObject currentHandObj = weapon.GetComponentInParent<Hand>().gameObject;
        Hand currentHandScript = currentHandObj.GetComponent<Hand>();

        // if not grip
        if (!SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip").GetStateDown(currentHandScript.handType)) { return; }

        // if right
        if (currentHandObj == rightHand)
        {
            if (null == rightWeapon) { PickupWeapon(rightHand, weapon); }
            else { DropWeapon(rightHand); }
        }
        // if left
        if (currentHandObj == leftHand)
        {
            if(null == leftWeapon) { PickupWeapon(leftHand, weapon); }
            else { DropWeapon(leftHand); }
        }

        SetClipGun();
    }

    // called by interactable on grip up
    public void OnDetach()
    {
        if (null != leftWeapon) { leftWeapon.GetComponent<Gun>().KeepParent(true); }
        if (null != rightWeapon) { rightWeapon.GetComponent<Gun>().KeepParent(false); }
    }

    public bool HandEmpty(Hand hand)
    {
        if (hand.gameObject == leftHand) { return null == leftWeapon; }
        if (hand.gameObject == rightHand) { return null == rightWeapon; }
        return true;
    }

    // set clip logic
    void SetClipGun()
    {
        if(null != leftWeapon && null != rightWeapon) { clipLogic.gunObject = null; }
        if(null != leftWeapon)
        {
            clipLogic.gunObject = leftWeapon;
            clipLogic.gunScript = leftWeapon.GetComponent<Gun>();
        }
        if(null != rightWeapon)
        {
            clipLogic.gunObject = rightWeapon;
            clipLogic.gunScript = rightWeapon.GetComponent<Gun>();
        }
    }

}
