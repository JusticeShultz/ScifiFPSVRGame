using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipLogic : MonoBehaviour
{
    //public Collider targetCollider;
    public string ClipName;
    public GameObject Gun;

    private void OnTriggerEnter(Collider collision)
    {
        print(collision.gameObject.name);

        //if (collision.collider == targetCollider)
        if (collision.gameObject.name == ClipName)
        {
            ++Gun.GetComponent<Gun>().BulletClips;
            Destroy(collision.gameObject);
        }
    }
}
