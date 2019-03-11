using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == "Spitter")
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
}
