using Assets.Scripts.Database.Entity;
using Photon.Pun;
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
            if (collision.CompareTag("Enemy") || collision.gameObject.tag == "Clone")
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

        if (collision.CompareTag("Player")
                && collision.gameObject.GetComponent<PlayerBase>().accountStatus == AccountStatus.PK
                && collision.gameObject.GetComponent<PhotonView>() != PV
                )
        {
            collision.GetComponent<PlayerBase>().TakeDamage(Damage);

            HitEffect = player_Pool.GetNormalAttack_Hit_FromPool();
            if (HitEffect != null)
            {
                HitEffect.transform.position = transform.position;
                HitEffect.SetActive(true);
            }
            TurnOff();
        }
    }

}
