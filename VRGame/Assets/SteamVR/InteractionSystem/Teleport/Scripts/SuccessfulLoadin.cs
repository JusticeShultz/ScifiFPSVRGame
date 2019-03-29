using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessfulLoadin : MonoBehaviour
{
    public static string prevScene;

	void Start ()
    {
        GameObject.Find("Fade").SetActive(false);	
	}
}
