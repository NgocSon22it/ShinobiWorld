using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kakashi_SkillTwo_FireExplosion : Boss_Skill
{
    new void OnEnable()
    {
        LifeTime = 1f;
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
           
        }
    }
}