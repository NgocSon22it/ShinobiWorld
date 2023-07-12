using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FierceFist : PlayerSkill
{
    new void OnEnable()
    {
        LifeTime = 5f;
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (AttackAble_Tag.Contains(collision.gameObject.tag))
        {
            if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Clone")
            {
                collision.GetComponent<Enemy>().TakeDamage(UserID, Damage);
            }
            if (collision.gameObject.tag == "Other")
            {
                collision.GetComponent<PlayerBase>().TakeDamage(Damage);
            }
            TurnOff();
        }
    }
}
