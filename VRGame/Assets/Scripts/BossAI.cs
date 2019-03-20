using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Animator animator;

    bool IsTurning = false;
    bool IsJumping = false;
    bool IsSpitting = false;
    bool IsUsingArtillary = false;
    bool IsStomping = false;

    void Start () { }
	
	void Update ()
    {
        animator.SetBool("IsTurning", IsTurning);
        animator.SetBool("IsJumping", IsJumping);
        animator.SetBool("IsSpitting", IsSpitting);
        animator.SetBool("IsUsingArtillary", IsUsingArtillary);
        animator.SetBool("IsStomping", IsStomping);
	}
}
