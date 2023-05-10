using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chakka : MonoBehaviour
{
    public Slider chakka;

    private int maxChakka = 100;

    private int currentChakka;

    public static Chakka instance;

    private WaitForSeconds regenTick = new WaitForSeconds(0.5f);

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentChakka = maxChakka;
        chakka.maxValue = maxChakka;
        chakka.value = maxChakka;

    }

    public void UseChakka(int amount)
    {
        if (currentChakka - amount >= 0)
        {
            currentChakka -= amount;
            chakka.value = currentChakka;

            StartCoroutine(RegenChakka());
        }
        else
        {
            Debug.Log("Not enough stamina");
        }
    }

    private IEnumerator RegenChakka()
    {
        yield return new WaitForSeconds(2);

        while (currentChakka < maxChakka)
        {
            currentChakka += maxChakka / 100;
            chakka.value = currentChakka;
            yield return regenTick;
        }
    }
}
