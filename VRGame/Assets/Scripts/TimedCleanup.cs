using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedCleanup : MonoBehaviour
{
    public float SecondsAlive = 5.0f;
    private int frames = 0;

	void Update ()
    {
        ++frames;

        if (frames >= SecondsAlive * 60)
            Destroy(gameObject);
	}
}
