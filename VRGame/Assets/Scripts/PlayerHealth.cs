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
    public Image damageImage;
    public float flashTime;

    public int deathCount;

    Color damageOrigColor;

    void Start()
    {
        deathCount = 0;
        damageOrigColor = damageImage.color;
        damageImage.color = Color.clear;
    }

    // Update is called once per frame
    void Update ()
    {
        // if (Input.GetMouseButtonDown(0)) { TakeDamage(); }
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, CurrentHealth / MaxHealth, 0.1f);

        if (CurrentHealth <= 0)
        {
            // print("I'm dead :O");
            // SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            // gameObject.transform.parent.transform.position = Vector3.zero;
            CurrentHealth = MaxHealth;
            SceneManager.LoadSceneAsync("Lose");
        }
    }

    public void TakeDamage()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, CurrentHealth / MaxHealth, 0.1f);
        StopCoroutine("DamageFlash");
        StartCoroutine("DamageFlash");
    }

    IEnumerator DamageFlash()
    {
        for(float i = 0; i < flashTime; i += Time.deltaTime)
        {
            damageImage.color = Color.Lerp(damageOrigColor, Color.clear, i / flashTime);
            yield return null;
        }
    }
}
