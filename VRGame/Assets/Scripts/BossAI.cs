using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Animator animator;
    public GameObject SmoothLook;

    bool IsTurning = false;
    bool IsJumping = false;
    bool IsSpitting = false;
    bool IsUsingArtillary = false;
    bool IsStomping = false;

    bool PlayerInView = false;

    GameObject Player;

    int StunTime = 0;

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

        if(StunTime > 0)
        {
            --StunTime;

            IsTurning = false;
            IsJumping = false;
            IsSpitting = false;
            IsUsingArtillary = false;
            IsStomping = false;
        }
        else if(PlayerInView && Vector3.Distance(Player.transform.position, transform.position) < 7)
        {
            IsStomping = true;
            IsTurning = false;
        }
        else
        {
            if (PlayerInView && Vector3.Distance(Player.transform.position, transform.position) > 7)
            {
                IsSpitting = true;
                IsTurning = false;
            }
            else
            {
                if (!PlayerInView && Vector3.Distance(Player.transform.position, transform.position) > 15)
                {
                    IsUsingArtillary = true;
                    IsTurning = false;
                    StunTime = 350;
                }
                else
                {
                    if (!PlayerInView && Vector3.Distance(Player.transform.position, transform.position) < 15)
                    {
                        IsJumping = true;
                        IsTurning = false;
                        StunTime = 900;
                    }
                    else
                    {
                        if (!PlayerInView)
                        {
                            if (Vector3.Distance(Player.transform.position, transform.position) > 5)
                            {
                                IsTurning = true;
                                SmoothLook.transform.LookAt(Player.transform.position);
                                transform.rotation = Quaternion.Lerp(transform.rotation, SmoothLook.transform.rotation, 0.1f);
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
