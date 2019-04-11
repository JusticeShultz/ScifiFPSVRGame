﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sets player position to the position of this gameObject
    // fixes error where player would not get set correctly in boss level
public class SetPlayerPos : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Vector3 thisPos = gameObject.transform.position;
        thisPos.y = 0;
        GameObject.Find("Player").transform.position = thisPos;
	}		
}
