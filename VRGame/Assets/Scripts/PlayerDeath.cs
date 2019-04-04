using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// resets player values at death, found in lose scene
public class PlayerDeath : MonoBehaviour {

    [Tooltip("Clip counts after player has died")]
    public int clips;
    [Tooltip("Where to display death statistics")]
    public UnityEngine.UI.Text statsText;

    GameObject player;
    PlayerHealth ph;
    WeaponHandler wh;

    int dc; // deathcount (I think storing like this is slightly more performant)
    string statsString;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.Find("Player");
        ph = player.GetComponentInChildren<PlayerHealth>();
        wh = player.GetComponent<WeaponHandler>();

        ph.deathCount++;
        dc = ph.deathCount;
        wh.leftWeapon = null;
        wh.rightWeapon = null;
        Gun.BulletClips = clips;
               
        statsString = dc == 1 ? "You have died " + dc + " time" : "You have died " + dc + " times";
        statsString += "\nHealth: " + ph.CurrentHealth + " / " + ph.maxHealth;
        statsString += "\nArmor: " + ph.armor;
        statsString += "\nClips: " + Gun.BulletClips;
        statsString += "\nRight Weapon: none";
        statsString += "\nLeft Weapon: none";

        statsText.text = statsString;
    }
	
}
