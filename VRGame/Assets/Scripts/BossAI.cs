using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public enum States { turn, artillery, jump, spit, stomp, cloak, idle };

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
    [Tooltip("What happens when we die?")]
    public GameObject deathExplosion;
    [Tooltip("Health bar image")]
    public UnityEngine.UI.Image healthBarImage;
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
    // [Range(10,100)]
    public int MaxHealth;
    //[Tooltip("Current health - public for testing only")]
    //public int Health;
    // public int MaxHealth { get { return _maxHealth; } set { if (value < 0) Debug.LogError("OH NO"); else _maxHealth = value; } }
    //[Tooltip("Health value where {globules} unlock")]
    //[Range(10,50)]
    //public int FinalStageUnlockValue;
    [Tooltip("{globules} are unlocked")]
    public bool FinalStage;
    [Tooltip("Explosion for when bullet does damage")]
    public GameObject damageExplosion;
    [Tooltip("Explosion for when bullet does not do damage")]
    public GameObject noDamageExplosion;
    [Tooltip("How much damage stomping does")]
    public int stompDamage = 30;
    [Tooltip("How many globules are initialized by default")]
    // how many globules are on
    public int maxGlobuleCount;
    [Header("Time to between animations")]
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
    [Header("Chances of state - try to add up to 100")]
        public int turnChance;
        public int artillaryChance;
        public int jumpChance;
        public int spitChance;
        public int stompChance;
        public int cloakChance;
        public int stunChance;


    int chanceTotal; // total of all cnahce numbers
    [System.NonSerialized]
    public int currentGlobuleCount; // current globule count

    // current animation state
    States state;

    public int Health;

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
    // jump animator
    Animator jumpAnimator;

    //How long until I can do these abilities again?
    float IdleCD = 0;
    float TurnCD = 0;
    float ArtillaryCD = 0;
    float JumpCD = 0;
    float SpitCD = 0;
    float StompCD = 0;
    float CloakCD = 0;
   
    float stompAnimTime = 0;
    bool triggeredJumpAnim;

    // globules on boss
    Globule[] Globules;
    // teleport points on back
    Valve.VR.InteractionSystem.TeleportPoint[] TeleportPoints;

    bool allowChange; // allow state to change

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
        stompAnimator = transform.Find("BossStomp").GetComponent<Animator>();
        jumpAnimator = transform.Find("BossJump").GetComponent<Animator>();

        IdleCD = 0;
        currentGlobuleCount = maxGlobuleCount;  

        chanceTotal = turnChance + artillaryChance + jumpChance + spitChance + stompChance + cloakChance + cloakChance;
        allowChange = true;
    }
	
	void Update ()
    {
        healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount, (float)Health / MaxHealth, 0.1f);

        // if globs gone
        if (currentGlobuleCount <= 0) { WinActions(); }

        // if out of bounds
        if (Vector3.Distance(Player.transform.position, transform.position) > 10) { return; }

        if(state == States.turn) { Turn(); } // keep turning

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
        if (CloakCD >= CloakTime) { state = States.idle; }

        if(!IsTurning && !IsUsingArtillary && !IsJumping && !IsSpitting && !IsStomping) { state = States.idle; }       

        SetAnimatorBools();

        if (state == States.idle && IdleCD >= IdleTime && allowChange)
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
    }

    // randomly chooses next state
    States GetNextState()
    {
        int rand = Random.Range(0, chanceTotal);
        int total = turnChance;

        if(rand < total) { return States.turn; }
        else if(rand < (total += artillaryChance)) { return States.artillery; }
        else if(rand < (total += jumpChance)) { return States.jump; }
        else if(rand < (total += spitChance)){ return States.spit; }
        else if(rand < (total += stompChance)) { return States.stomp; }
        else if (rand < (total += cloakChance)) { return States.cloak; }
        else { return States.idle; }
    }

    // change boss values for final fight stage
    void EnableFinalStage()
    {
        ReachedFinalStage = true;
        FinalStage = true;

        // make jump more likely
        jumpChance += 40;
        chanceTotal += 40;
        JumpTime *= 2;

        // enable rip-off globules
        for (int i = 0; i < maxGlobuleCount; i++) { Globules[i].gameObject.SetActive(true); }

        // open teleport points
        // foreach (var i in TeleportPoints) { i.locked = false; }        
    }

    // attempt to damage boss
    public void TryTakeBulletDamage(int Damage)
    {
        if (ReachedFinalStage) { AddGlobule(); }
        else { Health -= Damage; }

        if (Health <= 0) { EnableFinalStage(); }
    }

    // start win things
    void WinActions()
    {
        DoorObjectives.killedBoss = true; // open door

        // ultimately at this point the boss would roar and do some
        // kind of dying animation
        StartCoroutine("BossShrink");

        // trigger DeathAnim bool
    }

    // shrinks and explodes boss, death "animation"
    IEnumerator BossShrink()
    {
        for(float i = 0; i < 3; i+= Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, i / 3);
            yield return null;
        }

        Instantiate(deathExplosion, transform.position, transform.rotation);

        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }

    // returns correct bullet explosion
    public GameObject CurrentBulletExplosion()
    {
        if (FinalStage || state == States.cloak) { return noDamageExplosion; }
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
            case States.stomp:
                Stomp();
                break;
            case States.spit:
                Spit();
                break;
            case States.jump:
                Jump();
                break;
            case States.artillery:
                Artillery();
                break;
            case States.cloak:
                Ghost();
                break;
            default:
                // Turn();
                break;
        }
    }

    // throws player back and to last position
    void ThrowPlayer()
    {
        print("throw");
        Player.transform.parent.position = SavePrevPos.playerPrevPos;
        Player.GetComponent<Valve.VR.InteractionSystem.Player>().LastTPY = 0;
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
  
        //StunTime = 400; // 900
        //JumpCD = 2000; // 3500
        // ThrowPlayer();

        Renderer.material = Normal;
        allowChange = false;
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

        Renderer.material = Normal;
    }

    void Ghost()
    {
        IsTurning = false;
        IsStomping = false;
        IsSpitting = true;
        IsUsingArtillary = false;
        IsJumping = false;
        IsStompCyl = false;

        Renderer.material = Ghosted;
    }

    // sets animation states with controller bools
    void SetAnimatorBools()
    {
        //animator.SetBool("IsTurning", IsTurning);
        //animator.SetBool("IsJumping", IsJumping);
        //animator.SetBool("IsSpitting", IsSpitting);
        //animator.SetBool("IsUsingArtillary", IsUsingArtillary);
        //animator.SetBool("IsStomping", IsStomping);
        //stompAnimator.SetBool("IsStompCyl", IsStompCyl);

        animator.SetBool("IsTurning", state == States.turn);
        animator.SetBool("IsJumping", state == States.jump);
        animator.SetBool("IsSpitting", state == States.spit);
        animator.SetBool("IsUsingArtillary", state == States.artillery);
        animator.SetBool("IsStomping", state == States.stomp);
    }

    // called if shot with a weapon while in final stage
    void AddGlobule()
    {
        for(int i = maxGlobuleCount; i < Globules.Length; i++)
        {
            if (!Globules[i].gameObject.activeSelf)
            {
                Globules[i].gameObject.SetActive(true);
                currentGlobuleCount++;
                return;
            }
        }
    }

    // triggers jump damage
    public void JumpAnimEvent()
    {
        jumpAnimator.SetTrigger("TriggerJumpHit");
    }

    // triggers stomp damage
    public void StompAnimEvent()
    {
        stompAnimator.SetTrigger("IsStompCyl");
    }

    // allows state to change
    public void AllowChange()
    {
        allowChange = true;
    }
}
