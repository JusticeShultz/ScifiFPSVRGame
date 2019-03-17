using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script should be placed on interactables such as guns, etc that are necessary to do levels.

public class FailProofing : MonoBehaviour
{
    Vector3 StartPos;

	void Start ()
    {
        StartPos = transform.position;	
	}

    void Update ()
    {
        if (transform.position.y < -50)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = StartPos;
        }
    }
}
