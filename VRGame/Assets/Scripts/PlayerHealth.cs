﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth = 100;
    public float CurrentHealth = 100;
    public float Armor = 0;

    [Tooltip("The health bar image")]
    public /*UnityEngine.UI.*/Image healthBar;

    public static string sceneBeforeDeath;
	
	// Update is called once per frame
	void Update ()
    {
        healthBar.fillAmount = CurrentHealth / MaxHealth;

        if (CurrentHealth <= 0)
        {
            // print("I'm dead :O");
            // SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            // gameObject.transform.parent.transform.position = Vector3.zero;
            CurrentHealth = MaxHealth;
            SceneManager.LoadSceneAsync("Lose");
        }
    }
}
