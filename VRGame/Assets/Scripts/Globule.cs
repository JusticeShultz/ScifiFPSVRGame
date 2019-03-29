using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR.InteractionSystem;

public class Globule : MonoBehaviour {

    [Tooltip("Speed to return to original position if not thrown hard enough")]
    public float lerpSpeed;
    [Tooltip("How hard it needs to be thrown to go away")]
    public float throwVelocity;
    [Tooltip("How far away it needs to be to explode")]
    public float ExplodeDistance;
    [Tooltip("Explosion effect")]
    public GameObject explosion;

    Rigidbody rb;
    Throwable thr;
    Transform parent;

    Vector3 startPos;
    Vector3 origPos;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        thr = GetComponent<Throwable>();
        parent = transform.parent;

        thr.onPickUp.AddListener(OnGlobulePickup);
        thr.onDetachFromHand.AddListener(OnGlobuleRelease);

        origPos = transform.position;    

        ExplodeDistance += Vector3.Distance(transform.position, parent.position);

        // gameObject.SetActive(false); // start disabled 
    }

    void Update()
    {
        // if far enough from boss
        if(Vector3.Distance(transform.position, parent.position) > ExplodeDistance)
            StartCoroutine("Explode");

    }

    void OnEnable()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    // when object is picked up
    public void OnGlobulePickup()
    {
        // set rididbodyWasKinematic to true
        Hand.AttachedObject ao = (Hand.AttachedObject)GetComponentInParent<Hand>().currentAttachedObjectInfo;
        ao.attachedRigidbodyWasKinematic = false;

        StopCoroutine("FloatBack");
    }

    public void OnGlobuleRelease()
    {
        if(rb.velocity.magnitude < throwVelocity)
        {
            // lerp back to original position
            StartCoroutine("FloatBack");
        }
        else
        {
            // else throw
            rb.isKinematic = false;
            rb.useGravity = true;
        }        
    }

    // float back to original position
    IEnumerator FloatBack()
    {
        startPos = transform.position;
        transform.parent = parent;
        for(float i = 0; i < lerpSpeed; i += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos, origPos, i);
            yield return null;
        }
    }

    // exploding animation
    IEnumerator Explode()
    {
        BossAI.GlobuleCount--;
        Instantiate(explosion, transform.position, Quaternion.Euler(Vector3.zero));
        Destroy(this.gameObject);
        yield return null;
    }

}
