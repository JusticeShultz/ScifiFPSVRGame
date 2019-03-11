using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class DoorObjectives : MonoBehaviour
{
    public enum Types { KillAll, Kill, Find, Pickup, Destroy, etc }
    public Types Objective;
    public GameObject[] Kill;
    public string NameType;
    public GameObject TeleportPoint;
    private bool complete = false;
    
	void Update ()
    {
        if(Objective == Types.KillAll)
        {
            int kills = 0;

            for(int i = 0; i < Kill.Length; ++i)
            {
                if (Kill[i].name == NameType + "(Dead)") ++kills;
            }

            if (kills == Kill.Length) complete = true;
        }

        TeleportPoint.GetComponent<Valve.VR.InteractionSystem.TeleportPoint>().locked = !complete;
        TeleportPoint.GetComponent<Valve.VR.InteractionSystem.TeleportPoint>().UpdateVisuals();
    }
}
