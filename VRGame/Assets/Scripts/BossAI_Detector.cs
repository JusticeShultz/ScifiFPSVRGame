using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_Detector : MonoBehaviour
{
    public BossAI Handler;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "PlayerCollider")
        {
            Handler.EnterView();
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.name == "PlayerCollider")
        {
            Handler.ExitView();
        }
    }
}
