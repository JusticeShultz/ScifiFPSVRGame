﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using TMPro;

public class Gun : MonoBehaviour
{
    // [Tooltip("Which hand do we fire from?")]
        // public SteamVR_Input_Sources HandType;
    [Tooltip("What event makes us shoot?")]
        public SteamVR_Action_Boolean GrabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    [Tooltip("What event makes us reload?")]
        public SteamVR_Action_Boolean GrabGripAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    [Tooltip("How fast can we fire?")]
        public float FiringSpeed;
    [Tooltip("How many bullets do we have remaining?")]
        public int CurrentBulletCount = 24;
    [Tooltip("How many bullets can we store at maximum?")]
        public const int MaxBulletCount = 24;
    [Tooltip("How many bullets do we shoot at a time?")]
        public int BulletsShotAtOnce = 1;
    [Tooltip("Which bullet prefab should we use?")]
        public GameObject Bullet;
    [Tooltip("How fast should our bullet fly?")]
        public float BulletFlySpeed = 20;
    [Tooltip("How many bullet clips have we picked up?")]
        public static float BulletClips = 5;
    [Tooltip("Display text that shows your bullets and clip count.")]
        public GameObject DisplayText;
    [Tooltip("Time it takes to put new clip into gun and reload.")]
        public float reloadTime;
    [Tooltip("Offset from hand.")]
        public Vector3 posOffset;
        public Vector3 rotOffset;
        public Vector3 scaleOffset;

    public SteamVR_Action_Vibration hapticFlash = SteamVR_Input.GetAction<SteamVR_Action_Vibration>("Haptic");

    //For weapon changing add gun model, etc etc and just do object switches for pickups. (Push feature for later)

    //Shots per second that this weapon may fire.
    private float shotcooldown = 0.1f;
    // if this gun is currently held
    private bool activeGun;
    // can drop gun, resets on grip up after grabbing gun
    private bool canDrop;
    // hand object this script is attached to
    private GameObject hand;

    Transform parentObj;
    Rigidbody rb;
    Interactable interA;

    GameObject clip; // shows clip in gun
    Vector3 clipPos;
    Vector3 clipGoal;

    void Start ()
    {
        hand = GetComponentInParent<Hand>().gameObject;

        rb = GetComponent<Rigidbody>();
        interA = GetComponent<Interactable>();
        clip = transform.Find("ClipHolder").gameObject;
        clip.SetActive(false);
        clipPos = clip.transform.localPosition;
        clipGoal = transform.Find("ClipGoal").localPosition;

        // if player is holding this
        if (transform.parent != null && transform.parent.name == "HoverPoint")
        {
            activeGun = true;
            rb.useGravity = false;
            GetComponent<BoxCollider>().isTrigger = true;
            interA.enabled = false;
            interA.highlightOnHover = false;

            canDrop = true;
        }
        // gun not held
        else
        {
            activeGun = false;
            rb.useGravity = true;
            interA.enabled = true;
            interA.highlightOnHover = true;
            GetComponent<BoxCollider>().isTrigger = false;
            canDrop = false;
        }
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!activeGun) { return; }

        if (GrabPinchAction.GetStateDown(HandType))
            print("fire");
        if (GrabGripAction.GetStateDown(HandType))
            print("grip");

        if (GrabGripAction.GetStateUp(HandType)) { canDrop = true; }

        if (GrabGripAction.GetStateDown(HandType) && !activeGun && GetComponent<Interactable>().enabled && GetComponent<Interactable>().isHovering)
        {
            // PickupGun(GameObject.Find("RightHand").transform.Find("HoverPoint"));
            PickupGun(hand.transform.Find("HoverPoint"));
            return;
        }

        if (GrabGripAction.GetStateDown(HandType) && activeGun && canDrop)
        {
            DropGun();
            return;
        }

        ++shotcooldown;

        if (GrabPinchAction.GetState(HandType))
        {
            if(shotcooldown > FiringSpeed * 60 && CurrentBulletCount > 0)
            {
                shotcooldown = 0;
                CurrentBulletCount -= BulletsShotAtOnce;

                GameObject bullet = Instantiate(Bullet, transform.position, transform.rotation);
                bullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletFlySpeed;
                hapticFlash.Execute(0, 0.1f, 10.0f, 25, HandType);
            }
        }

        DisplayText.GetComponent<TextMeshPro>().text = CurrentBulletCount + "/" + MaxBulletCount + " (" + BulletClips + ")";
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (activeGun && (collision.name == "AmmoClip" || collision.name == "AmmoClip(Clone)"))
        {
            if (CurrentBulletCount >= MaxBulletCount) return;

            string ClipName = "AmmoClip";
            // if not grabbing a clip
            if (collision.gameObject.name == ClipName || collision.gameObject.name == ClipName + "(Clone)")
            {
                Destroy(collision.gameObject);
                StartCoroutine(LoadClip());
            }
        }
    }

    // move clip into gun
    IEnumerator LoadClip()
    {
        clip.SetActive(true); // show clip

        // drop old clip
        // GameObject oldClip = Instantiate<GameObject>(clip, clip.transform.position, clip.transform.rotation);
        // oldClip.GetComponent<Rigidbody>().useGravity = true;
        // oldClip.GetComponent<BoxCollider>().isTrigger = true;
        // oldClip.AddComponent<DestroyOnCollision>();

        for(float t = 0; t < 1; t += Time.deltaTime * reloadTime)
        {
            clip.transform.localPosition = Vector3.Lerp(clipPos, clipGoal, t);
            yield return null;
        }
        clip.SetActive(false);
        clip.transform.position = clipPos;
        CurrentBulletCount = MaxBulletCount; // or add bullet count on clip to partially refill
    }

    public void PickupGun(Transform parent)
    {
        interA.enabled = false;
        interA.highlightOnHover = false;
        GetComponent<BoxCollider>().isTrigger = true;
        rb.useGravity = false;
        rb.isKinematic = true;
        activeGun = true;
        canDrop = false;     

        // parent object reference
        parentObj = parent;
    }

    public void DropGun()
    {
        interA.enabled = true;
        interA.highlightOnHover = true;
        GetComponent<BoxCollider>().isTrigger = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        activeGun = false;

        // reset hand model

        parentObj = null;
        transform.parent = null;         
    }

    public void OnGripDown()
    {
        if (activeGun) { DropGun(); }
        else { PickupGun(GameObject.Find("RightHand").transform.Find("HoverPoint")); }
    }

    public void KeepParent()
    {
        transform.SetParent(parentObj);
        rb.isKinematic = true;
        transform.localPosition = posOffset;
        transform.localRotation = Quaternion.Euler(rotOffset);
    }
}
