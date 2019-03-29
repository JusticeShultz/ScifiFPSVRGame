﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth = 100;
    public float CurrentHealth = 100;
    public float Armor = 0;

    // ref to UI health bar
    UnityEngine.UI.Image healthBar;
	
	// Update is called once per frame
	void Update ()
    {
        healthBar.fillAmount = CurrentHealth / MaxHealth;

        if (CurrentHealth <= 0)
        {
            print("I'm dead :O");
            //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            gameObject.transform.parent.transform.position = Vector3.zero;
        }
    }
}
