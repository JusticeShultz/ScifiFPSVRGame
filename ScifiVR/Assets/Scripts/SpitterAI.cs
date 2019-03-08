using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterAI : MonoBehaviour {

    public GameObject Player;
    private UnityEngine.AI.NavMeshAgent Agent;

    void Start()
    {
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update ()
    {
        
        Agent.destination = Player.GetComponent<Transform>().position;
    }
}
