using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public enum States { turn, spit, jump, artillery, stomp, stun, cloak, idle };

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
    [Tooltip("What's our shooting material?")]
    public Material ShootingMat;
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
    [Tooltip("Time to idle between animations")]
    public float IdleTime = 5;
    public float TurnTime = 2;
    public float ArtillaryTime = 2;
    public float JumpTime = 2;
    public float SpitTime = 2;
    public float StompTime = 2;
    //How long are we disabled for?
    public float StunTime = 2;
    //How long does our cloak ability have left?
    public float CloakTime = 2;

    // current animation state
    States state;

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

    //How long until I can do these abilities again?
    float IdleCD = 0;
    float TurnCD = 0;
    float ArtillaryCD = 0;
    float JumpCD = 0;
    float SpitCD = 0;
    float StompCD = 0;
    float StunCD = 0;
    float CloakCD = 0;
   
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
        state = States.idle;
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

        IdleCD = 0;
    }
	
	void Update ()
    {
        // if globs gone
        // if (GlobuleCount <= 0) { WinActions(); }

        // if out of bounds
        if (Vector3.Distance(Player.transform.position, transform.position) > 10) { return; }

        // check if in active state
        //if (! animator.GetCurrentAnimatorStateInfo(0).IsName("Thresher_Idle"))
        //{
        //    idleCD = 0;


        //    // if(CloakCD >= CloakTime) 

        //    SetAnimatorBools();

        //    return;
        //}

        IdleCD += Time.deltaTime;
        TurnCD += Time.deltaTime;
        ArtillaryCD += Time.deltaTime;
        JumpCD += Time.deltaTime;
        SpitCD += Time.deltaTime;
        StompCD += Time.deltaTime;
        CloakCD += Time.deltaTime;

        if (TurnCD >= TurnTime) { IsTurning = false; }
        if (ArtillaryCD >= ArtillaryTime) { IsUsingArtillary = false; }
        if (JumpCD >= JumpTime) { IsJumping = false; }
        if (SpitCD >= SpitTime) { IsSpitting = false; }
        if (StompCD >= StompTime) { IsStomping = false; }

        if(!IsTurning && !IsUsingArtillary && !IsJumping && !IsSpitting && !IsStomping) { state = States.idle; }

        SetAnimatorBools();

        if (state == States.idle && IdleCD >= IdleTime)
        {
            // reset values
            IdleCD = 0;
            TurnCD = 0;
            ArtillaryCD = 0;
            JumpCD = 0;
            SpitCD = 0;
            StompCD = 0;
            CloakCD = 0;

            SetAnimatorBools();
            state = GetNextState();
            RunStateActions();
        }
        



        // print(Vector3.Distance(Player.transform.position, transform.position));

        // if (Input.GetMouseButtonDown(0)) { ThrowPlayer(); }

        

       
       
        /*
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
        if (!IsGhosted && !IsJumping && Vector3.Distance(Player.transform.position, transform.position) > .1) // 6
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
            Stomp();
        }
        else
        {
            Renderer.material = Normal;

            // spit
            if (Vector3.Distance(Player.transform.position, transform.position) > 4 && SpitCD <= 0) // 7
            {
                Spit();
            }
            // artillery
            else
            {
                if (Vector3.Distance(Player.transform.position, transform.position) > 8 && ArtillaryCD <= 0) // 15
                {
                    Artillery();
                }
                else
                {
                    // jump
                    if (Vector3.Distance(Player.transform.position, transform.position) < 15 && JumpCD <= 0)
                    {
                        Jump();
                    }
                    else
                    {
                        // turn
                        if (Vector3.Distance(Player.transform.position, transform.position) > .1) // 6
                        {
                            Turn();
                        }
                        else
                        {
                            IsTurning = false;
                            IsJumping = false;
                            IsSpitting = false;
                            IsUsingArtillary = false;
                            IsStomping = false;

                            Renderer.material = Normal;
                        }
                    }
                }
            }
        }
        */

        // enable {globules} if at health val and they have not yet been enabled
        if (Health <= FinalStageUnlockValue && !ReachedFinalStage) { EnableFinalStage(); }       
    }

    // randomly chooses next state
    States GetNextState()
    {
        if(Random.Range(0, 3) == 0) { return States.turn; }
        return (States)Random.Range(0, 6);
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

    // runs functions associated with current state
    void RunStateActions()
    {
        switch (state)
        {
            case States.turn:
                Turn();
                break;
            case States.stun:
                break;
            case States.stomp:
                Stomp();
                break;
            case States.spit:
                Spit();
                break;
            case States.jump:
                Jump();
                break;
            case States.cloak:
                break;
            case States.artillery:
                Artillery();
                break;
            default:
                // Turn();
                break;
        }
    }

    // throws player off back and to last position
    void ThrowPlayer()
    {
        Player.transform.parent.position = SavePrevPos.playerPrevPos;
    }

    void Turn()
    {
        IsTurning = true;
        IsStomping = false;
        IsSpitting = false;
        IsUsingArtillary = false;
        IsJumping = false;
        IsStompCyl = false;

        SmoothLook.transform.LookAt(Player.transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, SmoothLook.transform.rotation, 0.01f);

        Renderer.material = Normal;
    }

    void Jump()
    {
        IsTurning = false;
        IsStomping = false;
        IsSpitting = false;
        IsUsingArtillary = false;
        IsJumping = true;
        IsStompCyl = false;
        //StunTime = 400; // 900
        //JumpCD = 2000; // 3500
        // ThrowPlayer();

        Renderer.material = Normal;
    }

    void Artillery()
    {
        IsTurning = false;
        IsStomping = false;
        IsSpitting = false;
        IsUsingArtillary = true;
        IsJumping = false;
        IsStompCyl = false;
        //StunTime = 125;
        //ArtillaryCD = 1450;
        Instantiate(ArtillaryType, SpitPoint.transform.position + (Vector3.up * 6), SpitPoint.transform.rotation);
        Instantiate(ArtillaryType, SpitPoint.transform.position + (Vector3.up * 8), SpitPoint.transform.rotation);
        Instantiate(ArtillaryType, SpitPoint.transform.position + (Vector3.up * 10), SpitPoint.transform.rotation);
        Instantiate(ArtillaryType, SpitPoint.transform.position + (Vector3.up * 12), SpitPoint.transform.rotation);
        Instantiate(ArtillaryType, SpitPoint.transform.position + (Vector3.up * 14), SpitPoint.transform.rotation);

        Renderer.material = ShootingMat;
    }

    void Spit()
    {
        IsTurning = false;
        IsStomping = false;
        IsSpitting = true;
        IsUsingArtillary = false;
        IsJumping = false;
        IsStompCyl = false;
        SpitCD = 55;
        GameObject bullet = Instantiate(ShotType, SpitPoint.transform.position, SpitPoint.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = SpitPoint.transform.right * -10 + (Vector3.up * 2);

        Renderer.material = ShootingMat;
    }

    void Stomp()
    {
        IsTurning = false;
        IsStomping = true;
        IsSpitting = false;
        IsUsingArtillary = false;
        IsJumping = false;
        //StompCD = 370;
        IsStompCyl = true;
        stompAnimTime = 2;

        Renderer.material = Normal;
    }
    void SetAnimatorBools()
    {
        animator.SetBool("IsTurning", IsTurning);
        animator.SetBool("IsJumping", IsJumping);
        animator.SetBool("IsSpitting", IsSpitting);
        animator.SetBool("IsUsingArtillary", IsUsingArtillary);
        animator.SetBool("IsStomping", IsStomping);
        stompAnimator.SetBool("IsStompCyl", IsStompCyl);
    }
}
