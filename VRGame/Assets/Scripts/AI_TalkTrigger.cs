using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_TalkTrigger : MonoBehaviour
{
    public AudioClip ToSay;
    public GameObject ToEmit;
    public int Delay = 4;
    bool toggled = false;

    public void Trigger()
    {
        if (!toggled)
        {
            toggled = true;
            StartCoroutine(DoTask());
        }
    }

    IEnumerator DoTask()
    {
        yield return new WaitForSeconds(Delay);
        ToEmit.GetComponent<AudioSource>().Play();
    }
}