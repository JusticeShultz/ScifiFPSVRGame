using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterAI : MonoBehaviour {

    public GameObject Player;
    public GameObject Directional;
    public GameObject ShotType;

    public int Health;
    private UnityEngine.AI.NavMeshAgent Agent;

    void Start()
    {
        if(Player == null) Player = GameObject.Find("VRCamera");

        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
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

        if (Health <= 0)
        {
            animator.SetBool("IsDead", true);
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BoxCollider>().enabled = false;
            Agent.isStopped = true;
            gameObject.name = "Spitter(Dead)";
            return;
        }

        if (dist < 20)
        {
            if (dist > 7)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                animator.SetBool("IsAttacking", false);
                animator.SetBool("IsMoving", true);
                Agent.destination = position;
                Agent.isStopped = false;
            }
            else
            {
                //RaycastHit hitInfo;
                //if (Physics.Raycast(transform.position, Directional.transform.forward, out hitInfo, 100.0f))
                animator.SetBool("IsAttacking", true);
                animator.SetBool("IsMoving", false);
                GetComponent<Rigidbody>().isKinematic = true;
                Agent.isStopped = true;
                GetComponent<Transform>().LookAt(position, Vector3.up);
                //Do enemy shooting here.
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

/*private bool Triggered = false;
 * 
 * if (!Triggered)
{
    Agent.destination = position;
    animator.SetBool("IsMoving", true);
    Directional.transform.LookAt(position, Vector3.up);
    RaycastHit hitInfo;
    GetComponent<Rigidbody>().isKinematic = false;

    if (Physics.Raycast(transform.position, Directional.transform.forward, out hitInfo, 100.0f))
        if (hitInfo.collider.gameObject.name == "Player")
            Triggered = true;

    Debug.DrawLine(transform.position, hitInfo.point);
}*/
