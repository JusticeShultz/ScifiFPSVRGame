using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessfulLoadin : MonoBehaviour
{
	void Start ()
    {
        GameObject fade = GameObject.Find("Fade");
        if (null != fade) { fade.SetActive(false); }
	}
}
