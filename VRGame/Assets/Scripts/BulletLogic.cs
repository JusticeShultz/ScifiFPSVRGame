using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public bool IsEnemyShot = false;

    void OnCollisionEnter(Collision col)
    {
        if (!IsEnemyShot)
        {
            if (col.gameObject.name == "Spitter")
            {
                //Do damage to it
                col.gameObject.GetComponent<SpitterAI>().Health -= 25;
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (col.gameObject.name == "PlayerCollider")
            {
                //Do damage to it
                col.gameObject.GetComponent<PlayerHealth>().CurrentHealth -= Mathf.Clamp((15.0f - col.gameObject.GetComponent<PlayerHealth>().Armor), 1.0f, 10000.0f);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
