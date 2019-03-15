using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableEntity : MonoBehaviour
{
    public int Health = 35;
    public GameObject DeathEffect;

	void Update ()
    {
		if(Health <= 0)
        {
            if (DeathEffect != null) Instantiate(DeathEffect, transform.position, transform.rotation);

            Destroy(gameObject);
        }
	}
}
