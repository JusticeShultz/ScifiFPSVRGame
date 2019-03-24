using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoBodyCollisions : MonoBehaviour
{
    public bool IsClip = false;

	void Awake ()
    {
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GameObject.Find("PlayerCollider").GetComponent<Collider>());
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GameObject.Find("BodyCollider").GetComponent<Collider>());
        if(!IsClip) Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GameObject.Find("ClipInventory").GetComponent<Collider>());
    }
}