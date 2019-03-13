using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject Player;
    public float Distance = 8.0f;

    private void Start()
    {
        Player = GameObject.Find("VRCamera");    
    }

    void Update ()
    {
		if(Vector3.Distance(Player.transform.position, gameObject.transform.position) < Distance)
            GetComponent<Animator>().SetBool("DoorOpen", true);
        else GetComponent<Animator>().SetBool("DoorOpen", false);
    }
}
