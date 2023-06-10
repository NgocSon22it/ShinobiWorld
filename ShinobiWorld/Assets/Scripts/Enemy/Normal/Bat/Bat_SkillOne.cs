using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_SkillOne : Boss_Skill
{
    new void OnEnable()
    {
        LifeTime = 3f;
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
            if (collision.gameObject.tag == "Player")
            {
                collision.GetComponent<PlayerBase>().TakeDamage(Damage);
            }
            Destroy(gameObject);
        }
    }

}
