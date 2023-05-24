using Assets.Scripts.Database.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDart : MonoBehaviour
{
    [SerializeField] List<string> ListTag = new List<string>();

    string UserID;
    AccountSkill_Entity accountSkill_Entity;

    public void SetUpRedDart(string UserID, AccountSkill_Entity accountSkill_Entity)
    {
        this.UserID = UserID;
        this.accountSkill_Entity = accountSkill_Entity;
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
            if (collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<Enemy>().TakeDamage(UserID, accountSkill_Entity.Damage);
            }
            TurnOff();
        }
    }
}
