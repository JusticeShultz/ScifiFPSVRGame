using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;
using UnityEngine.VR;
using Valve.VR.InteractionSystem;

// controls holding of weapons
// 3 weapons - right, left, holster

public class WeaponHandler : MonoBehaviour {

    [Tooltip("Weapon in left hand")]
    public GameObject leftWeapon;
    [Tooltip("Weapon in right hand")]
    public GameObject rightWeapon;
    [Tooltip("Weapon holstered at hip")] // not yet implemented
    GameObject holsterWeapon;
    [Tooltip("Grip button state has changed")]
    public bool GrabDelta = false;

    GameObject leftHand;
    GameObject rightHand;   

    ClipLogic clipLogic; // controls whether player is holding clip

	void Start ()
    {
        leftHand = GameObject.Find("LeftHand");
        rightHand = GameObject.Find("RightHand");
        clipLogic = GetComponentInChildren<ClipLogic>();
    }

    // pick up a weapon
    // param hand = hand that is picking up
    // param weapon = weapon to be picked up
    void PickupWeapon(GameObject hand, GameObject weapon)
    {
        weapon.GetComponent<Gun>().PickupGun(hand.transform.Find("HoverPoint"));
        weapon.GetComponent<Gun>().HandType = hand.GetComponent<Hand>().handType;
        if (hand == leftHand) { leftWeapon = weapon; }
        if (hand == rightHand) { rightWeapon = weapon; }

        Hand.AttachedObject ao = ((Hand.AttachedObject)hand.GetComponent<Hand>().currentAttachedObjectInfo);
        ao.attachedRigidbodyWasKinematic = false;

        weapon.GetComponent<Gun>().KeepParent(hand == leftHand);
    }

    // drop weapon
    // param hand = hand to be dropped from
    void DropWeapon(GameObject hand)
    {
        hand.GetComponentInChildren<Gun>().DropGun();
        if (hand == rightHand) { rightWeapon = null; }
        if (hand == leftHand) { leftWeapon = null; }
    }

    // called by interactable on grip down
    // param weapon = the interactable this is called on
    public void OnPickup(GameObject weapon)
    {
        GameObject currentHandObj = weapon.GetComponentInParent<Hand>().gameObject;
        Hand currentHandScript = currentHandObj.GetComponent<Hand>();

        // if not grip
        if (!SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip").GetStateDown(currentHandScript.handType))
        {
            // GrabDelta = false;
            return;
        }

        if (GrabDelta) return;
        else GrabDelta = true;

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

        GrabDelta = false;
    }

    // returns whether hand is holding weapon
    // param hand = the hand to check
    public bool HandEmpty(Hand hand)
    {
        if (hand.gameObject == leftHand) { return null == leftWeapon; }
        if (hand.gameObject == rightHand) { return null == rightWeapon; }
        return true;
    }

    // set clip logic so that gun hand cannot pick up ammo
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
