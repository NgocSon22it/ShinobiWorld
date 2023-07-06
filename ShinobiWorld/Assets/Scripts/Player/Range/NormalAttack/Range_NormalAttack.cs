using Assets.Scripts.Database.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Range_NormalAttack : PlayerSkill
{
    new void OnEnable()
    {
        LifeTime = 1.5f;
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
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Enemy>().TakeDamage(UserID, Damage);

                HitEffect = player_Pool.GetNormalAttack_Hit_FromPool();
                if (HitEffect != null)
                {
                    HitEffect.transform.position = transform.position;
                    HitEffect.SetActive(true);
                }
            }
            TurnOff();
        }
    }

}
