using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using TMPro;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
        public int MaxBulletCount = 24;
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
    [Tooltip("Offset from hand. Position")]
        public Vector3 posOffset;
    [Tooltip("Offset from hand. Rotation")]
        public Vector3 rotOffset;
    [Tooltip("Offset from hand. Scale")]
        public Vector3 scaleOffset;
    [Tooltip("Is this gun a hair trigger pull? (If we hold the trigger will it keep firing?)")]
        public bool IsAutomatic = true;
    [Tooltip("Where does our shot fire from?")]
        public GameObject GunBarrel;

    public SteamVR_Action_Vibration hapticFlash = SteamVR_Input.GetAction<SteamVR_Action_Vibration>("Haptic");

    //For weapon changing add gun model, etc etc and just do object switches for pickups. (Push feature for later)

    // Shots per second that this weapon may fire.
    private float shotcooldown = 0.1f;
    // if this gun is currently held
    public  bool activeGun;
    // can drop gun, resets on grip up after grabbing gun
    private bool canDrop;


    // hand object this script is attached to
    // private GameObject hand;
    private bool shotdelta = false;
    private Rigidbody rigidbodyComponent;

    Transform parentObj;
    Rigidbody rb;
    Interactable interA;
    BoxCollider bc;
    Throwable th;

    GameObject clip; // shows clip in gun
    Vector3 clipPos; // current clip position (for loading anim)
    Vector3 clipGoal; // goal clip position (for loading anim)

    WeaponHandler weaponHandler; // handles picking up weapons in specific hands

    void Start ()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
        weaponHandler = GameObject.Find("Player").GetComponent<WeaponHandler>();

        rb = GetComponent<Rigidbody>();
        interA = GetComponent<Interactable>();
        bc = GetComponent<BoxCollider>();
        th = GetComponent<Throwable>();
        clip = transform.Find("ClipHolder").gameObject;
        clip.SetActive(false);
        clipPos = clip.transform.localPosition;
        clipGoal = transform.Find("ClipGoal").localPosition;

        // initialize pickup event
        // this needs to be dynamically set because a new player os loading in to each scene
        // th.onPickUp.AddListener(delegate { weaponHandler.OnPickup(this.gameObject); } );
        // th.onDetachFromHand.AddListener(weaponHandler.OnDetach);
        th.onPickUp.AddListener(OnPickup);
        th.onDetachFromHand.AddListener(OnDetach);


        // if player is holding this
        if (transform.parent != null && transform.parent.name == "HoverPoint")
        {
            // activeGun = true;
            Activate();
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
            Deactivate();
            rb.useGravity = true;
            interA.enabled = true;
            interA.highlightOnHover = true;
            GetComponent<BoxCollider>().isTrigger = false;
            canDrop = false;
        }

        // add input listener to rifle
        WeaponHandler wh = GameObject.Find("Player").GetComponent<WeaponHandler>();
        //GetComponent<Throwable>().onPickUp.AddListener(delegate { wh.OnPickup(this.gameObject); });

        //GetComponent<Throwable>().onDetachFromHand.AddListener(delegate { wh.OnDetach(); });
    }

    // set fields for gun being held
    void Activate()
    {
        activeGun = true;
    }

    // set fields for gun not beign held
    void Deactivate()
    {
        activeGun = false;
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

        if (!activeGun)
        {
            rigidbodyComponent.isKinematic = false;
            rigidbodyComponent.useGravity = true;
            return;
        } // if not being held

        // helpful debugging
        if (GrabPinchAction.GetStateDown(HandType))
            print("fire");
        if (GrabGripAction.GetStateDown(HandType))
            print("grip");

        // if (GrabGripAction.GetStateUp(HandType)) { canDrop = true; }       

        // drop weapon
        //if (activeGun && canDrop && GrabGripAction.GetStateDown(HandType))
        //{
        //    weaponHandler.OnPickup(gameObject);
        //    return;
        //}

        if (!GrabGripAction.GetStateDown(HandType))
            weaponHandler.GrabDelta = false;

        ++shotcooldown;

        if (IsAutomatic)
        {
            if (GrabPinchAction.GetState(HandType))
            {
                if (shotcooldown > FiringSpeed * 60 && CurrentBulletCount > 0)
                {
                    shotcooldown = 0;
                    CurrentBulletCount -= BulletsShotAtOnce;

                    GameObject bullet = Instantiate(Bullet, GunBarrel.transform.position, transform.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = GunBarrel.transform.forward * BulletFlySpeed;
                    hapticFlash.Execute(0, 0.1f, 100.0f, 25, HandType);
                }
            }
        }
        else
        {
            if (GrabPinchAction.GetState(HandType))
            {
                if (shotcooldown > FiringSpeed * 60 && CurrentBulletCount > 0 && !shotdelta)
                {
                    shotdelta = true;
                    shotcooldown = 0;
                    CurrentBulletCount -= BulletsShotAtOnce;

                    GameObject bullet = Instantiate(Bullet, GunBarrel.transform.position, transform.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = GunBarrel.transform.forward * BulletFlySpeed;
                    hapticFlash.Execute(0, 0.1f, 10.0f, 25, HandType);
                }
            }
            else shotdelta = false;
        }

        DisplayText.GetComponent<TextMeshPro>().text = CurrentBulletCount + "/" + MaxBulletCount + " (" + BulletClips + ")";
    }

    private void OnTriggerEnter(Collider collision)
    {
        print("Object attempted to go in clip: " + collision.gameObject.name);
        // reload
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

    // pick up this gun
    // param parent = the object to child this to
    public void PickupGun(Transform parent)
    {
        print("pick up");

        interA.enabled = false;
        interA.highlightOnHover = false;
        bc.isTrigger = true;
        rb.useGravity = false;
        rb.isKinematic = true; // not this
        // activeGun = true;
        Activate();
        canDrop = false;     

        // parent object reference
        parentObj = parent;

        KeepParent(parentObj);

        // GetComponent<Throwable>().enabled = false;
    }

    public void DropGun()
    {
        print("drop");

        interA.enabled = true;
        interA.highlightOnHover = true;
        bc.isTrigger = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        print("ik = f");
        // activeGun = false;
        Deactivate();

        // reset hand model

        parentObj = null;
        transform.SetParent(null);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        // GetComponent<Throwable>().enabled = true;
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

    // keeps the parent object the same, needed because on grip up, interactable script automatically unparents object
    // param leftHanded = whether gun is going to left hand - used to negate offset
    public void KeepParent(bool leftHanded)
    {
        if (!activeGun) { return; }

        transform.SetParent(parentObj);
        rb.isKinematic = true; // not this
        
        if (leftHanded)
        {
            Vector3 newOffset = posOffset;
            newOffset.x *= -1;
            transform.localPosition = newOffset;
        }
        else { transform.localPosition = posOffset; }

        transform.localRotation = Quaternion.Euler(rotOffset);
    }

    // throwable.onPickup
    public void OnPickup()
    {
        if (GrabPinchAction.GetState(HandType)) { return; }
        if (!GrabGripAction.GetStateDown(HandType)) { return; }

        if (!activeGun)
        {
            canDrop = false;
            PickupGun(transform.parent);
            weaponHandler.PickupWeapon(transform.parent.gameObject, this.gameObject);
        }
        else { canDrop = true; }
    }

    // throwable.onDetach
    public void OnDetach()
    {
        // if (GrabPinchAction.GetState(HandType)) { return; }
        if (GrabPinchAction.GetStateUp(HandType)) { return; }
        if (!GrabGripAction.GetStateUp(HandType)) { return; }

        print("hit");

        if (canDrop)
        {
            canDrop = false;
            DropGun();
            weaponHandler.DropWeapon(transform.parent.gameObject);
        }
        else { KeepParent(parentObj); }

        canDrop = true;
    }

}
