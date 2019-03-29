using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechAI : MonoBehaviour
{
    private GameObject Player;
    private PlayerHealth PlayerHealth;
    public float AttackRate = 0.5f;
    public float Damage = 10.0f;
    public int Health;
    public Animator Animator;
    private UnityEngine.AI.NavMeshAgent Agent;
    private float AttackCD = 0;
    private DestroyableEntity entityComponent;
    
    void Start()
    {
        Player = GameObject.Find("VRCamera");
        PlayerHealth = GameObject.Find("PlayerCollider").GetComponent<PlayerHealth>();

        if(Animator == null)
            Animator = GetComponent<Animator>();

        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        entityComponent = GetComponent<DestroyableEntity>();
    }

    void Update ()
    {
        Health = entityComponent.Health;
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
            Animator.SetBool("IsDead", true);
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
            Agent.isStopped = true;
            gameObject.name = "Leech(Dead)";
            return;
        }

        if (dist < 16)
        {
            if (dist > 2 && Vector3.Distance(Player.transform.position, transform.position) > 2)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                Animator.SetBool("IsAttacking", false);
                Animator.SetBool("IsMoving", true);
                Agent.destination = position;
                Agent.isStopped = false;
            }
            else
            {
                Animator.SetBool("IsAttacking", true);
                Animator.SetBool("IsMoving", false);
                GetComponent<Rigidbody>().isKinematic = true;
                Agent.isStopped = true;
                transform.LookAt(position, Vector3.up);
               
                //Attack

                if (Vector3.Distance(Player.transform.position, transform.position) < 2 && AttackCD <= 0)
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
            Animator.SetBool("IsAttacking", false);
            Animator.SetBool("IsMoving", false);
        }
    }
}