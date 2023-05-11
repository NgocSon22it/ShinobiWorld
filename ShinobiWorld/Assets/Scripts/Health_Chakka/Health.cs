using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public Slider health;

    private int maxHealth = 100;

    private int currentHealth;

    public static Health instance;

    private WaitForSeconds regenTick = new WaitForSeconds(2f);

    public void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        currentHealth = maxHealth;
        health.maxValue = maxHealth;
        health.value = maxHealth;

    }

    public void UseHealth(int amount)
    {
        if (currentHealth - amount >= 0 )
        {
            currentHealth -= amount;
            health.value = currentHealth;

            StartCoroutine(RegenStamina());
        }
        else
        {
            Debug.Log("Not enough stamina");
        }
    }

    private IEnumerator RegenStamina()
    {
        yield return new  WaitForSeconds(5);

        while (currentHealth < maxHealth)
        {
            currentHealth += maxHealth / 100;
            health.value = currentHealth;
            yield return regenTick;
        }
    }
}
