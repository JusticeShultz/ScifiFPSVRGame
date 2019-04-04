using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Tooltip("What animator are we going to play our states on?")]
    public Animator animator;
    [Tooltip("Which object will do a direct look at for us to lerp to?")]
    public GameObject SmoothLook;
    [Tooltip("What prefab do we shoot from the mouth?")]
    public GameObject ShotType;
    [Tooltip("What prefab do we shoot in mass from the top?")]
    public GameObject ArtillaryType;
    [Tooltip("Where is our mouth at?")]
    public GameObject SpitPoint;
    [Tooltip("Who is rendering us?")]
    public SkinnedMeshRenderer Renderer;
    [Tooltip("What's our normal material?")]
    public Material Normal;
    [Tooltip("What's our stunned material?")]
    public Material Stunned;
    [Tooltip("What's our cloaked material?")]
    public Material Ghosted;
    [Tooltip("Are we cloaked?")]
    public bool IsGhosted = false;
    [Tooltip("Maximum health")]
    [Range(10,100)]
    public int MaxHealth;
    //[Tooltip("Current health - public for testing only")]
    //public int Health;
    // public int MaxHealth { get { return _maxHealth; } set { if (value < 0) Debug.LogError("OH NO"); else _maxHealth = value; } }
    [Tooltip("Health value where {globules} unlock")]
    [Range(10,50)]
    public int FinalStageUnlockValue;
    [Tooltip("{globules} are unlocked")]
    public bool FinalStage;
    [Tooltip("Explosion for when bullet does damage")]
    public GameObject damageExplosion;
    [Tooltip("Explosion for when bullet does not do damage")]
    public GameObject noDamageExplosion;
    [Tooltip("How much damage stomping does")]
    public int stompDamage = 30;

    int Health;

    //Are we turning?
    bool IsTurning = false;
    //Are we jumping?
    bool IsJumping = false;
    //Are we spitting?
    bool IsSpitting = false;
    //Are we using our artillary ability?
    bool IsUsingArtillary = false;
    //Are we stomping?
    bool IsStomping = false;
    // Is stomp cylinder anim playing?
    bool IsStompCyl = false;

    // final stage has been unlocked
    bool ReachedFinalStage;

    //Who is the player?
    GameObject Player;    
    //What is my collider?
    Collider myCollider;
    //boss stomp cylinder
    GameObject stompAnim;
    // stomp animator
    Animator stompAnimator;

    //How long are we disabled for?
    int StunTime = 0;

    //How long until I can do these abilities again?
    int ArtillaryCD = 0;
    int JumpCD = 0;
    int SpitCD = 0;
    int StompCD = 0;
    int CloakCD = 0;

    //How long does our cloak ability have left?
    int CloakTime = 0;
    float stompAnimTime = 0;

    // how many globules are on
    [System.NonSerialized]
    public static int GlobuleCount;
    // globules on boss
    Globule[] Globules;
    // teleport points on back
    Valve.VR.InteractionSystem.TeleportPoint[] TeleportPoints;

    void Start ()
    {
        Health = MaxHealth;
        
        myCollider = GetComponent<Collider>();
        Player = GameObject.Find("PlayerCollider");
        stompAnim = transform.Find("BossStomp").gameObject;
        FinalStage = false;
        ReachedFinalStage = false;
        Globules = GetComponentsInChildren<Globule>();
        foreach (var i in Globules) { i.gameObject.SetActive(false); }          
        Reference[] refs = GetComponentsInChildren<Reference>();
        TeleportPoints = new Valve.VR.InteractionSystem.TeleportPoint[refs.Length];
        for (int i = 0; i < refs.Length; i++) { TeleportPoints[i] = refs[i].referenceType.GetComponent<Valve.VR.InteractionSystem.TeleportPoint>(); }           
        GlobuleCount = Globules.Length;
        stompAnimator = transform.Find("BossStomp").GetComponent<Animator>();
    }
	
	void Update ()
    {
        // print(Vector3.Distance(Player.transform.position, transform.position));

        // if (Input.GetMouseButtonDown(0)) { ThrowPlayer(); }

        if (GlobuleCount <= 0) { WinActions(); }    
        
        if(stompAnimTime > 0)
        {
            stompAnimTime -= Time.deltaTime;
        }
        else { IsStompCyl = false; }

        ++CloakCD;

        if(CloakCD > 3500)
        {
            CloakCD = 0;
            CloakTime = 450;
            ThrowPlayer();
        }

        if(CloakTime > 0)
        {
            --CloakTime;
            IsGhosted = true;
        }
        else IsGhosted = false;

        if (IsGhosted)
            Renderer.material = Ghosted;
        else Renderer.material = Normal;
        myCollider.enabled = !IsGhosted;

        animator.SetBool("IsTurning", IsTurning);
        animator.SetBool("IsJumping", IsJumping);
        animator.SetBool("IsSpitting", IsSpitting);
        animator.SetBool("IsUsingArtillary", IsUsingArtillary);
        animator.SetBool("IsStomping", IsStomping);
        stompAnimator.SetBool("IsStompCyl", IsStompCyl);

        --JumpCD;
        --ArtillaryCD;
        --SpitCD;
        --StompCD;

        // always turn
        if (Vector3.Distance(Player.transform.position, transform.position) > .1) // 6
        {
            IsTurning = true;
            // IsStomping = false;
            // IsSpitting = false;
            SmoothLook.transform.LookAt(Player.transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, SmoothLook.transform.rotation, 0.01f);
        }

        // after jump or artillery atack
        if (StunTime > 0)
        {
            --StunTime;

            IsTurning = false;
            IsJumping = false;
            IsSpitting = false;
            IsUsingArtillary = false;
            IsStomping = false;

            Renderer.material = Stunned;
        }
        // stomp
        else if(Vector3.Distance(Player.transform.position, transform.position) < 7 && StompCD <= 0)
        {
            IsStomping = true;
            IsTurning = false;
            IsSpitting = false;
            StompCD = 370;
            IsStompCyl = true;
            stompAnimTime = 2;

            Renderer.material = Normal;
        }
        else
        {
            Renderer.material = Normal;

            // spit
            if (Vector3.Distance(Player.transform.position, transform.position) > 4 && SpitCD <= 0) // 7
            {
                IsSpitting = true;
                IsTurning = false;
                IsStomping = false;
                IsSpitting = false;
                SpitCD = 55;
                GameObject bullet = Instantiate(ShotType, SpitPoint.transform.position, SpitPoint.transform.rotation);
                bullet.GetComponent<Rigidbody>().velocity = SpitPoint.transform.right * -10 + (Vector3.up * 2);
            }
            // artillery
            else
            {
                if (Vector3.Distance(Player.transform.position, transform.position) > 8 && ArtillaryCD <= 0) // 15
                {
                    IsUsingArtillary = true;
                    IsTurning = false;
                    IsStomping = false;
                    IsSpitting = false;
                    StunTime = 125;
                    ArtillaryCD = 1450;
                    Instantiate(ArtillaryType, SpitPoint.transform.position + (Vector3.up * 6), SpitPoint.transform.rotation);
                    Instantiate(ArtillaryType, SpitPoint.transform.position + (Vector3.up * 8), SpitPoint.transform.rotation);
                    Instantiate(ArtillaryType, SpitPoint.transform.position + (Vector3.up * 10), SpitPoint.transform.rotation);
                    Instantiate(ArtillaryType, SpitPoint.transform.position + (Vector3.up * 12), SpitPoint.transform.rotation);
                    Instantiate(ArtillaryType, SpitPoint.transform.position + (Vector3.up * 14), SpitPoint.transform.rotation);
                }
                else
                {
                    // jump
                    if (Vector3.Distance(Player.transform.position, transform.position) < 15 && JumpCD <= 0)
                    {
                        IsJumping = true;
                        IsTurning = false;
                        IsStomping = false;
                        IsSpitting = false;
                        StunTime = 900;
                        JumpCD = 3500;
                        ThrowPlayer();
                    }
                    else
                    {
                        // turn
                        if (Vector3.Distance(Player.transform.position, transform.position) > .1) // 6
                        {
                            IsTurning = true;
                            IsStomping = false;
                            IsSpitting = false;
                            SmoothLook.transform.LookAt(Player.transform.position);
                            transform.rotation = Quaternion.Lerp(transform.rotation, SmoothLook.transform.rotation, 0.01f);
                        }
                        else
                        {
                            IsTurning = false;
                            IsJumping = false;
                            IsSpitting = false;
                            IsUsingArtillary = false;
                            IsStomping = false;
                        }
                    }
                }
            }
        }

        // enable {globules} if at health val and they have not yet been enabled
        if (Health <= FinalStageUnlockValue && !ReachedFinalStage) { EnableFinalStage(); }       
    }

    // change boss values for final fight stage
    void EnableFinalStage()
    {
        ReachedFinalStage = true;
        FinalStage = true;

        // enable rip-off globules
        foreach (var i in Globules) { i.gameObject.SetActive(true); }

        // open teleport points
        foreach (var i in TeleportPoints) { i.locked = false; }        
    }

    // attempt to damage boss
    public void TryTakeBulletDamage(int Damage)
    {
        if (ReachedFinalStage) return; // no bullet damage in final stage
        Health -= Damage;
    }

    // start win things
    void WinActions()
    {
        DoorObjectives.killedBoss = true; // open door

        // ultimately at this point the boss would roar and do some
        // kind of dying animation

        // trigger DeathAnim bool
    }

    // returns correct bullet explosion
    public GameObject CurrentBulletExplosion()
    {
        if (FinalStage || IsGhosted) { return noDamageExplosion; }
        else { return damageExplosion; }
    }

    // throws player off back and to last position
    void ThrowPlayer()
    {
        Player.transform.parent.position = SavePrevPos.playerPrevPos;
    }

}
