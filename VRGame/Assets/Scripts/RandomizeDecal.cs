using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeDecal : MonoBehaviour
{
    void Start ()
    {
        transform.localPosition = new Vector3(0, Random.Range(0.002f, 0.1f), 0);
	}
}
