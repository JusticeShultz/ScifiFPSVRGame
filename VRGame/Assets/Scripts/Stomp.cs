using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles damage from stomp animation
public class Stomp : MonoBehaviour {

    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "PlayerCollider")
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
