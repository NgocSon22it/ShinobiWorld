using Assets.Scripts.Database.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField] List<string> ListTag = new List<string>();

    PlayerBase playerBase;
    AccountWeapon_Entity accountWeapon_Entity;

    public void SetUpDart(PlayerBase playerBase, AccountWeapon_Entity accountWeapon_Entity)
    {
        this.playerBase = playerBase;
        this.accountWeapon_Entity = accountWeapon_Entity;
    }

    private void OnEnable()
    {
        Invoke(nameof(TurnOff), 5f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void TurnOff()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ListTag.Contains(collision.gameObject.tag))
        {
            if(collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<Enemy>().TakeDamage(playerBase, accountWeapon_Entity.Damage);
            }
            TurnOff();
        }
    }
}
