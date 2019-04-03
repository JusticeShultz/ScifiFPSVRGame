using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecalibrateHeight : MonoBehaviour {

    Camera VRCam;

    // Use this for initialization
    void Start() {
        VRCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update() {     
        // change key later
        if (Input.GetMouseButtonDown(0)) { Recalibrate(); }
    }

    void Recalibrate()
    {
        Vector3 currentPos = VRCam.transform.position;
    }
}
