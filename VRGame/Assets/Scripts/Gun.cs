using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using TMPro;
using System;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    [Tooltip("Which hand do we fire from?")]
        public SteamVR_Input_Sources HandType;
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
        public static float BulletClips = 2;
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
    public  bool activeGun;
    // can drop gun, resets on grip up after grabbing gun
    private bool canDrop;
    // hand object this script is attached to
    // private GameObject hand;

    Transform parentObj;
    Rigidbody rb;
    Interactable interA;
    BoxCollider bc;

    GameObject clip; // shows clip in gun
    Vector3 clipPos;
    Vector3 clipGoal;

    // manually resize box collider
    Vector3 activeBCPos;
    Vector3 activeBCSize;
    Vector3 inactiveBCPos;
    Vector3 inactiveBCSize;


    WeaponHandler weaponHandler;

    void Start ()
    {
        // hand = GetComponentInParent<Hand>().gameObject;
        weaponHandler = GameObject.Find("Player").GetComponent<WeaponHandler>();

        rb = GetComponent<Rigidbody>();
        interA = GetComponent<Interactable>();
        bc = GetComponent<BoxCollider>();
        clip = transform.Find("ClipHolder").gameObject;
        clip.SetActive(false);
        clipPos = clip.transform.localPosition;
        clipGoal = transform.Find("ClipGoal").localPosition;

        // I know, I know, disgusting. I can't figure out Teleprt.cs
        activeBCPos = new Vector3(2.282737e-05f, 0.0001128286f, 0.005854433f);
        activeBCSize = new Vector3(0.005684052f, 0.009154602f, 0.01072443f);
        inactiveBCPos = new Vector3(-0.0002120605f, 0.0001128157f, 0.001436021f);
        inactiveBCSize = new Vector3(0.005175263f, 0.009154602f, 0.01956122f);

        // if player is holding this
        if (transform.parent != null && transform.parent.name == "HoverPoint")
        {
            // activeGun = true;
            Active();
            rb.useGravity = false;
            GetComponent<BoxCollider>().isTrigger = true;
            interA.enabled = false;
            interA.highlightOnHover = false;

            canDrop = true;
        }
        // gun not held
        else
        {
            // activeGun = false;
            Inactive();
            rb.useGravity = true;
            interA.enabled = true;
            interA.highlightOnHover = true;
            GetComponent<BoxCollider>().isTrigger = false;
            canDrop = false;
        }

        // add listener to rifle
        WeaponHandler wh = GameObject.Find("Player").GetComponent<WeaponHandler>();
        GetComponent<Throwable>().onPickUp.AddListener(delegate { wh.OnPickup(this.gameObject); });
        GetComponent<Throwable>().onDetachFromHand.AddListener(delegate { wh.OnDetach(); });
    }

    void Active()
    {
        activeGun = true;
        bc.center = activeBCPos;
        bc.size = activeBCSize;
    }

    void Inactive()
    {
        activeGun = false;
        bc.center = inactiveBCPos;
        bc.size = inactiveBCSize;
    }

    // Update is called once per frame
    void Update ()
    {
        //if (GrabGripAction.GetStateDown(HandType) && !activeGun && GetComponent<Interactable>().enabled && GetComponent<Interactable>().isHovering)
        //{
        //    PickupGun(GameObject.Find("RightHand").transform.Find("HoverPoint"));
        //    // PickupGun(hand.transform.Find("HoverPoint"));
        //    return;
        //}

        if (!activeGun) { return; }

        if (GrabPinchAction.GetStateDown(HandType))
            print("fire");
        if (GrabGripAction.GetStateDown(HandType))
            print("grip");

        // if (GrabGripAction.GetStateUp(HandType)) { canDrop = true; }       

        // drop weapon
        if (GrabGripAction.GetStateDown(HandType) && activeGun && canDrop)
        {
            weaponHandler.OnPickup(gameObject);
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
        if (activeGun && ((collision.name == "AmmoClip(Clone)" && GrabClip.holdingClip ) || collision.name == "AmmoClip") && !clip.activeSelf)
        {
            if (CurrentBulletCount >= MaxBulletCount) return;

            Destroy(collision.gameObject);
            StartCoroutine(LoadClip());
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
        bc.isTrigger = true;
        rb.useGravity = false;
        rb.isKinematic = true; // not this
        // activeGun = true;
        Active();
        canDrop = false;     

        // parent object reference
        parentObj = parent;
    }

    public void DropGun()
    {
        interA.enabled = true;
        interA.highlightOnHover = true;
        bc.isTrigger = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        // activeGun = false;
        Inactive();

        // reset hand model

        parentObj = null;
        transform.SetParent(null);
    }

    //public void OnGripDown()
    //{
    //    //if (activeGun) { DropGun(); }
    //    //else { PickupGun(GameObject.Find("RightHand").transform.Find("HoverPoint")); }
    //    weaponHandler.OnPickup(this.gameObject);
    //}

    //public void OnGripUp()
    //{
    //    weaponHandler.OnDetach();
    //}

    public void KeepParent(bool leftHanded)
    {
        if (!activeGun) { return; }

        canDrop = true;

        transform.SetParent(parentObj);
        rb.isKinematic = true; // not this
        
        if (leftHanded)
        {
            Vector3 newOffset = posOffset;
            newOffset.x *= -1;
            // transform.localPosition = new Vector3(-posOffset.x, posOffset.y, posOffset.z);
            transform.localPosition = newOffset;
        }
        else { transform.localPosition = posOffset; }
        // transform.localPosition = posOffset;
        transform.localRotation = Quaternion.Euler(rotOffset);
    }
}
