using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithOffset : MonoBehaviour
{
    public GameObject Follow;
    public Vector3 Offset;
    public bool FixedOffset = true;
    public float OffsetAmount = -0.3f;
    public bool IsClip = false;
    public GameObject Head;

    void Update ()
    {
        var fixedOffset = Vector3.zero;

        if (!IsClip)
        {
            if (FixedOffset)
                fixedOffset = Follow.GetComponent<Transform>().right * OffsetAmount;
            GetComponent<Transform>().position = Follow.GetComponent<Transform>().position + Offset + fixedOffset;
        }
        else
        {
            RaycastHit hit;
            Ray ray = new Ray();
            ray.origin = Follow.transform.position;
            ray.direction = -Vector3.up;

            if (Physics.Raycast(ray, out hit, 100))
            {
                //Offset = (Follow.transform.position + Head.transform.position) / 2;
            }
                //Offset = 
            fixedOffset = Follow.GetComponent<Transform>().right * OffsetAmount;
            GetComponent<Transform>().position = Follow.GetComponent<Transform>().position + Offset + fixedOffset;
        }
	}
}
