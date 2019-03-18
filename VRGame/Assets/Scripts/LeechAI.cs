using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechAI : MonoBehaviour
{
    public GameObject Player;
    public PlayerHealth PlayerHealth;
    public int Health;
    public float AttackRate = 0.5f;
    public float Damage = 10.0f;
    private UnityEngine.AI.NavMeshAgent Agent;
    private float AttackCD = 0;

    void Start()
    {
        if(Player == null) Player = GameObject.Find("VRCamera");

        PlayerHealth = GameObject.Find("PlayerCollider").GetComponent<PlayerHealth>();
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update ()
    {
        Animator animator = GetComponent<Animator>();

        float dist = 0.0f;

        Vector3 position;
        position = Player.transform.position;
        Agent.destination = position;

        Vector3[] corners = Agent.path.corners;

        for (int c = 0; c < corners.Length - 1; ++c)
        {
            dist += Mathf.Abs((corners[c] - corners[c + 1]).magnitude);
        }

        if(AttackCD > 0) --AttackCD;

        if (Health <= 0)
        {
            animator.SetBool("IsDead", true);
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
            Agent.isStopped = true;
            gameObject.name = "Leech(Dead)";
            return;
        }

        if (dist < 30)
        {
            GetComponent<Transform>().LookAt(position, Vector3.up);

            if (dist > 2)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                animator.SetBool("IsAttacking", false);
                animator.SetBool("IsMoving", true);
                Agent.destination = position;
                Agent.isStopped = false;
            }
            else
            {
                animator.SetBool("IsAttacking", true);
                animator.SetBool("IsMoving", false);
                GetComponent<Rigidbody>().isKinematic = true;
                Agent.isStopped = true;

                //Attack
               
                if(Vector3.Distance(Player.transform.position, transform.position) < 5 && AttackCD <= 0)
                {
                    PlayerHealth.CurrentHealth -= Mathf.Clamp(Damage - PlayerHealth.Armor, 1, float.MaxValue);
                    AttackCD = AttackRate * 60;
                }
            }
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
            Agent.isStopped = true;
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsMoving", false);
        }
    }
}