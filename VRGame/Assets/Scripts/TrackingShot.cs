using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingShot : MonoBehaviour
{
    GameObject Player;

	void Start ()
    {
        Player = GameObject.Find("PlayerCollider");
	}
	
	void Update ()
    {
        transform.LookAt(Player.transform.position);
        GetComponent<Rigidbody>().velocity = transform.forward * 10;
	}
}
