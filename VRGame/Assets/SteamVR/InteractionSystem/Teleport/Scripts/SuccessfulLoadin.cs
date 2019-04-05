using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessfulLoadin : MonoBehaviour
{
	void Start ()
    {
        GameObject.Find("ScreenFade").GetComponent<SpriteRenderer>().enabled = false;
	}
}