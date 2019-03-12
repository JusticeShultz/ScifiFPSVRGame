using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithOffset : MonoBehaviour
{
    public GameObject Follow;
    public Vector3 Offset;
    public bool FixedOffset = true;

	void Update ()
    {
        var fixedOffset = Vector3.zero;

        if (FixedOffset)
            fixedOffset = Follow.GetComponent<Transform>().right * -0.3f;
        GetComponent<Transform>().position = Follow.GetComponent<Transform>().position + Offset + fixedOffset;
	}
}
