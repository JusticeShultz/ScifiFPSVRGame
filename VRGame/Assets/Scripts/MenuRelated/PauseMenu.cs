using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PauseMenu : MonoBehaviour {

    public SteamVR_Action_Boolean pauseAction;
    public Hand hand;

    bool paused;

	// Use this for initialization
	void Start () {
        paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnPauseActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
    {
        if (newValue) { TryPause(); }
    }

    void TryPause()
    {
        if (!paused) { Pause(); }
        else { UnPause(); }
    }

    void Pause()
    {
        Time.timeScale = 0;
        print("paused");
    }

    void UnPause()
    {
        Time.timeScale = 1;
        print("unpaused");
    }
}
