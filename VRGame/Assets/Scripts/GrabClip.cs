using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using UnityEngine.SceneManagement;

// grab clip from belt 

public class GrabClip : MonoBehaviour
{
    public SteamVR_Action_Boolean grabClipAction;
    public GameObject newClipPrefab;
    public bool buttonDown;

    public static bool holdingClip;

    Hand hand;  
    ClipLogic clip;
    BoxCollider pickUpPrecision;

    [System.NonSerialized]
    public GameObject grabbedClip;
    [System.NonSerialized]
    public Gun gun;

    WeaponHandler weaponHandler;

    private void OnEnable()
    {
        holdingClip = false;
        weaponHandler = GetComponentInParent<WeaponHandler>();
        hand = GetComponentInParent<Hand>();
        clip = GameObject.Find("ClipInventory").GetComponent<ClipLogic>();
        pickUpPrecision = GameObject.Find("ClipInventory").GetComponent<BoxCollider>();

        if (hand == null)
            hand = this.GetComponent<Hand>();

        if (grabClipAction == null)
        {
            Debug.LogError("<b>[SteamVR Interaction]</b> No grabClip action assigned");
            return;
        }

        grabClipAction.AddOnChangeListener(OnClipActionChange, hand.handType);

    }

    private void OnDisable()
    {
        if (grabClipAction != null)
            grabClipAction.RemoveOnChangeListener(OnClipActionChange, hand.handType);
    }

    private void OnClipActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
    {
        if (!weaponHandler.HandEmpty(hand)) { return; } // if hand holding gun

        buttonDown = newValue;
        if (newValue && /*gun.CurrentBulletCount <= Gun.MaxBulletCount && */ Gun.BulletClips > 0 && pickUpPrecision.bounds.Contains(hand.transform.position))
        {
            GenerateNewClip();
        }

        if (!newValue) { DropClip(); }
    }

    public void GenerateNewClip()
    {
        holdingClip = true;
        grabbedClip = Instantiate<GameObject>(newClipPrefab);
        grabbedClip.transform.position = hand.transform.position;
        grabbedClip.transform.SetParent(hand.transform);
        grabbedClip.GetComponent<Rigidbody>().useGravity = false;
        Gun.BulletClips--;
        clip.canPutBack = false;
    }

    void DropClip()
    {
        holdingClip = false;
        if (null != grabbedClip)
        {
            grabbedClip.GetComponent<Rigidbody>().useGravity = true;
            grabbedClip.transform.SetParent(null);
            SceneManager.MoveGameObjectToScene(grabbedClip, SceneManager.GetActiveScene());
            grabbedClip = null;
        }
    }
}


