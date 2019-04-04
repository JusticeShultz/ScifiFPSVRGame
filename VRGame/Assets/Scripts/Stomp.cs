using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles damage from stomp animation
public class Stomp : MonoBehaviour {

    int damage;
    private void Start()
    {
        damage = GetComponentInParent<BossAI>().stompDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "PlayerCollider")
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
            print("should red flash");
        }
    }
}
