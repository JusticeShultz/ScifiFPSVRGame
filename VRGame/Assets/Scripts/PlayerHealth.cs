﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

// controls player health and manages red damage flash
public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    private float currentHealth = 100;
    public float armor = 0;

    public float CurrentHealth { get { return currentHealth; } }

    [Tooltip("The health bar image")]
    public Image healthBar;
    public Image damageImage;
    public float flashTime;

    public int deathCount;

    Color damageOrigColor;

    void Start()
    {
        deathCount = 0;
        damageOrigColor = damageImage.color;
        damageImage.color = Color.clear;
        damageImage.gameObject.SetActive(true); // usually disabled while editing

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update ()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentHealth / maxHealth, 0.1f);

        //if (currentHealth <= 0)
        //{
        //    // currentHealth = maxHealth;
        //    GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<LoadSceneAsync>().Do("Lose");
        //}
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // loads death scene
            GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<LoadSceneAsync>().Do("Lose");
        }
        
        StopCoroutine("DamageFlash");
        StartCoroutine("DamageFlash");       
    }

    // flashes screen red
    IEnumerator DamageFlash()
    {
        for(float i = 0; i < flashTime; i += Time.deltaTime)
        {
            damageImage.color = Color.Lerp(damageOrigColor, Color.clear, i / flashTime);
            yield return null;
        }
    }

    public void resetHealth()
    {
        currentHealth = maxHealth;
    }
}
