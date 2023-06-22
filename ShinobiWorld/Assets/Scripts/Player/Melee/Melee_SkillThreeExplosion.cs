using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_SkillThreeExplosion : PlayerSkill
{
    new void OnEnable()
    {
        LifeTime = 0.8f;
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
            if (collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<Enemy>().TakeDamage(UserID, Damage);
            }
        }
    }
}
