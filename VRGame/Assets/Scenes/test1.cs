using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using Valve.VR.InteractionSystem;
using Valve.VR;
using UnityEngine.Events;

public class test1 : MonoBehaviour {

    public string[] names;
    
    
	// Use this for initialization
	void Start () {
        names = Input.GetJoystickNames();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown(names[0])) { Debug.Log("worked"); }
        
	}
}
