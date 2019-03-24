using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Animator animator;
    public GameObject SmoothLook;
    public GameObject ShotType;
    public GameObject SpitPoint;

    bool IsTurning = false;
    bool IsJumping = false;
    bool IsSpitting = false;
    bool IsUsingArtillary = false;
    bool IsStomping = false;

    bool PlayerInView = false;

    GameObject Player;

    int StunTime = 0;
    int ArtillaryCD = 0;
    int JumpCD = 0;
    int SpitCD = 0;
    int StompCD = 0;

    void Start ()
    {
        Player = GameObject.Find("PlayerCollider");
    }
	
	void Update ()
    {
        animator.SetBool("IsTurning", IsTurning);
        animator.SetBool("IsJumping", IsJumping);
        animator.SetBool("IsSpitting", IsSpitting);
        animator.SetBool("IsUsingArtillary", IsUsingArtillary);
        animator.SetBool("IsStomping", IsStomping);

        --JumpCD;
        --ArtillaryCD;
        --SpitCD;
        --StompCD;

        if (StunTime > 0)
        {
            --StunTime;

            IsTurning = false;
            IsJumping = false;
            IsSpitting = false;
            IsUsingArtillary = false;
            IsStomping = false;
        }
        else if(Vector3.Distance(Player.transform.position, transform.position) < 7 && StompCD <= 0)
        {
            IsStomping = true;
            IsTurning = false;
            StompCD = 370;
        }
        else
        {
            if (PlayerInView && Vector3.Distance(Player.transform.position, transform.position) > 7 && SpitCD <= 0)
            {
                IsSpitting = true;
                IsTurning = false;
                SpitCD = 125;
                GameObject bullet = Instantiate(ShotType, SpitPoint.transform.position, SpitPoint.transform.rotation);
                bullet.GetComponent<Rigidbody>().velocity = SpitPoint.transform.right * 10 + (Vector3.up * 2);
            }
            else
            {
                if (!PlayerInView && Vector3.Distance(Player.transform.position, transform.position) > 15 && ArtillaryCD <= 0)
                {
                    IsUsingArtillary = true;
                    IsTurning = false;
                    StunTime = 125;
                    ArtillaryCD = 1250;
                }
                else
                {
                    if (!PlayerInView && Vector3.Distance(Player.transform.position, transform.position) < 15 && JumpCD <= 0)
                    {
                        IsJumping = true;
                        IsTurning = false;
                        StunTime = 900;
                        JumpCD = 3500;
                    }
                    else
                    {
                        if (!PlayerInView)
                        {
                            if (Vector3.Distance(Player.transform.position, transform.position) > 6)
                            {
                                IsTurning = true;
                                SmoothLook.transform.LookAt(Player.transform.position);
                                transform.rotation = Quaternion.Lerp(transform.rotation, SmoothLook.transform.rotation, 0.005f);
                            }
                            else
                            {
                                IsTurning = false;
                            }
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
    }

    public void EnterView()
    {
        PlayerInView = true;
    }

    public void ExitView()
    {
        PlayerInView = false;
    }
}
