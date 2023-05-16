using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    // Entity
    protected Boss_Entity boss_Entity = new Boss_Entity();
    int CurrentHealth;

    //Health UI
    [SerializeField] Image CurrentHealth_UI;


    public void Start()
    {
        boss_Entity = Boss_DAO.GetBossByID(boss_Entity.ID);
        CurrentHealth = boss_Entity.Health;
        LoadHealthUI();
    }

    public void LoadHealthUI()
    {
        CurrentHealth_UI.fillAmount = (float)CurrentHealth / (float)boss_Entity.Health;
    }

    public void TakeDamage(PlayerBase playerBase, int Damage)
    {
        CurrentHealth -= Damage;
        LoadHealthUI();
        if(CurrentHealth <= 0)
        {
            playerBase.HealAmountOfHealth(10);
        }
    }

}
