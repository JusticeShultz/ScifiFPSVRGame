using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterAI : MonoBehaviour {

    public GameObject Player;
    public GameObject Directional;
    public int Health;

    private UnityEngine.AI.NavMeshAgent Agent;
    private bool Triggered = false;

    void Start()
    {
        if(Player == null) Player = GameObject.Find("VRCamera");

        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 position;
        position = Player.GetComponent<Transform>().position;

        Animator animator = GetComponent<Animator>();

        if (Health <= 0)
        {
            animator.SetBool("IsDead", true);
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
            Agent.isStopped = true;
            gameObject.name = "Spitter(Dead)";
            return;
        }

        if (!Triggered)
        {
            Agent.destination = position;
            animator.SetBool("IsMoving", true);
            Directional.GetComponent<Transform>().LookAt(position, Vector3.up);
            RaycastHit hitInfo;
            GetComponent<Rigidbody>().isKinematic = false;
            if (Physics.Raycast(GetComponent<Transform>().position, Directional.GetComponent<Transform>().forward, out hitInfo, 100.0f))
                if(hitInfo.collider.gameObject.name == "Player")
                    Triggered = true;
        }
        else
        {
            if (Agent.remainingDistance < 11)
            {
                RaycastHit hitInfo;

                if (Physics.Raycast(GetComponent<Transform>().position, Directional.GetComponent<Transform>().forward, out hitInfo, 100.0f))
                    animator.SetBool("IsAttacking", true);
                else animator.SetBool("IsMoving", false);

                GetComponent<Rigidbody>().isKinematic = true;
                Agent.isStopped = true;
                GetComponent<Transform>().LookAt(position, Vector3.up);
                //Do enemy shooting here.
            }
            else
            {
                GetComponent<Rigidbody>().isKinematic = false;
                animator.SetBool("IsMoving", true);
                Agent.destination = position;
            }
        }
    }
}
