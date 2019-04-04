using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class DoorObjectives : MonoBehaviour
{
    public enum Types { KillAll, Kill, Find, Pickup, Destroy, Boss, Debug }
    public Types Objective;
    public GameObject[] Kill;
    public string NameType;
    public GameObject TeleportPoint;
    private bool complete = false;

    public static bool killedBoss = false;
    
	void Update ()
    {
        if (Objective == Types.KillAll)
        {
            int kills = 0;

            if (Kill.Length == 0) return;
            if (Kill[0] == null) return;

            for (int i = 0; i < Kill.Length; ++i)
            {
                if (Kill[i].name == NameType + "(Dead)") ++kills;
            }

            if (kills == Kill.Length) complete = true;
        }

        if(killedBoss && Objective == Types.Boss)
            complete = true;

        if (Objective == Types.Debug)
        {
            try
            {
                TeleportPoint.GetComponent<Valve.VR.InteractionSystem.TeleportPoint>().locked = !complete;
                TeleportPoint.GetComponent<Valve.VR.InteractionSystem.TeleportPoint>().UpdateVisuals();
            }
            catch { print("teleportpoint == null"); }
        }
        else
        {
            TeleportPoint.GetComponent<Valve.VR.InteractionSystem.TeleportPoint>().locked = !complete;
            TeleportPoint.GetComponent<Valve.VR.InteractionSystem.TeleportPoint>().UpdateVisuals();
        }
    }
}