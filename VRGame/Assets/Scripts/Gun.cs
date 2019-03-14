using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using TMPro;

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
        public float BulletClips = 5;
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
    public bool activeGun;
    // script on right hand to pick up / drop weapons
    SwitchGun switchGunScript;

    Rigidbody rb;

    GameObject clip; // shows clip in gun
    Vector3 clipPos;
    Vector3 clipGoal;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        clip = transform.Find("ClipHolder").gameObject;
        clip.SetActive(false);
        clipPos = clip.transform.localPosition;
        clipGoal = transform.Find("ClipGoal").localPosition;
        switchGunScript = GameObject.Find("RightHand").transform.GetComponentInChildren<SwitchGun>();

        // if player is holding this
        if (transform.parent != null && transform.parent.name == "HoverPoint")
        {
            activeGun = true;
            rb.useGravity = false;
            GetComponent<BoxCollider>().enabled = false;
        }
        // gun not held
        else
        {
            activeGun = false;
            rb.useGravity = true;
            // gameObject.AddComponent<Interactable>();
            gameObject.AddComponent(typeof(Interactable));
            gameObject.AddComponent<Throwable>();
            gameObject.AddComponent<VelocityEstimator>();

            // load material -> throwable

            GetComponent<BoxCollider>().enabled = true;
        }
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!activeGun) { return; }

        if (GrabPinchAction.GetStateDown(HandType))
            print("fire");
        if (GrabGripAction.GetStateDown(HandType))
            print("reload");

        ++shotcooldown;

        if (GrabPinchAction.GetState(HandType)) //print("firing");
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

        if(GetComponent<Interactable>() != null && GetComponent<Interactable>().attachedToHand) { switchGunScript.gunObj = this.gameObject; }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (activeGun)
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
        // check if new gun
        else
        {
            // switchGunScript.gunObj = this.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switchGunScript.overGun = false;
    }

    // move clip into gun
    IEnumerator LoadClip()
    {
        clip.SetActive(true); // show clip

        // drop old clip
        GameObject oldClip = Instantiate<GameObject>(clip, clip.transform.position, clip.transform.rotation);
        oldClip.GetComponent<Rigidbody>().useGravity = true;
        oldClip.GetComponent<BoxCollider>().isTrigger = true;
        oldClip.AddComponent<DestroyOnCollision>();

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
        Destroy(GetComponent<Interactable>());
        rb.useGravity = false;
        GetComponent<BoxCollider>().enabled = false;
        activeGun = true;

        // reset local transform
        transform.SetParent(parent);
        transform.localPosition = posOffset;
        transform.localRotation = Quaternion.Euler(rotOffset);
        transform.localScale = scaleOffset;
    }

    public void DropGun()
    {
        gameObject.AddComponent<Interactable>();
        GetComponent<BoxCollider>().enabled = true;
        rb.useGravity = true;
        activeGun = false;

        // reset hand model

        transform.parent = null;         
    }
}
