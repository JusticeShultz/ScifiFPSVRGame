using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR.InteractionSystem;

public class ClipLogic : MonoBehaviour
{
    //public Collider targetCollider;
    public string ClipName;
    public GrabClip grabClipAction;

    [System.NonSerialized]
    public GameObject gunObject;
    [System.NonSerialized]
    public Gun gunScript;

    Interactable clipInteractable;
    public bool canPutBack;

    private void Start()
    {
        clipInteractable = GetComponent<Interactable>();
        // gunScript = gunObject.GetComponent<Gun>();
        canPutBack = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        print(collision.gameObject.name);

        if (collision.gameObject.name == "BossViewRange")
            collision.gameObject.GetComponent<BossAI_Detector>().ForceEnter();

        // if not grabbing a clip
        if (canPutBack && (collision.gameObject.name == ClipName || collision.gameObject.name == ClipName + "(Clone)"))
        {
            ++Gun.BulletClips;
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "BossViewRange")
            collision.gameObject.GetComponent<BossAI_Detector>().ForceExit();

        if (collision.gameObject.name == ClipName || collision.gameObject.name == ClipName + "(Clone)")
        {
            canPutBack = true;
        }
    }

}
