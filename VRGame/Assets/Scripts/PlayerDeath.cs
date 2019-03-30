using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// resets player values at death
public class PlayerDeath : MonoBehaviour {

    [Tooltip("Clip counts after player has died")]
    public int clips;
    [Tooltip("WHere to display death statistics")]
    public UnityEngine.UI.Text statsText;

    GameObject player;
    PlayerHealth ph;
    WeaponHandler wh;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Player");
        ph = player.GetComponentInChildren<PlayerHealth>();
        wh = player.GetComponent<WeaponHandler>();

        ph.deathCount++;
        wh.leftWeapon = null;
        wh.rightWeapon = null;
        Gun.BulletClips = clips;

        statsText.text = "You have died " + ph.deathCount + " times";
    }
	
}
