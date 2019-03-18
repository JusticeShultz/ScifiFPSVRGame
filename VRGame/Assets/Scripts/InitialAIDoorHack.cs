using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialAIDoorHack : MonoBehaviour
{
    private GameObject Player;
    private Reference PlayerRef;
    private bool DoOnce = false;

    private void Start()
    {
        Player = GameObject.Find("Player");
        PlayerRef = Player.GetComponent<Reference>();
    }

    void Update ()
    {
        if(!DoOnce && PlayerRef.referenceType.activeSelf && Vector3.Distance(transform.position, Player.transform.position) < 10)
        {
            DoOnce = true;
            GetComponent<Animator>().SetBool("IsOpen", true);
            Destroy(GetComponent<BoxCollider>());
        }
	}
}
