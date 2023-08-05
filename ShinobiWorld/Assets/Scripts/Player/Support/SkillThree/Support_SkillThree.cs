using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support_SkillThree : PlayerSkill
{
    Quaternion rotation;

    public void SetUpDirection(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation = rotation;
        //transform.Rotate(0, 0, 0);
    }

    new void OnEnable()
    {
        LifeTime = 2f;
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
                collision.GetComponent<Enemy>().TakeDamage(PV.ViewID, Damage);             
            }
            HitEffect = player_Pool.GetSkillThree_Hit_FromPool();
            if (HitEffect != null)
            {
                HitEffect.transform.position = transform.position;
                HitEffect.SetActive(true);
            }
            TurnOff();
        }

        if (collision.CompareTag("Player")
                && collision.gameObject.GetComponent<PlayerBase>().accountStatus == AccountStatus.PK
                && collision.gameObject.GetComponent<PhotonView>() != PV
                )
        {
            collision.GetComponent<PlayerBase>().TakeDamage(Damage);

            HitEffect = player_Pool.GetSkillThree_Hit_FromPool();
            if (HitEffect != null)
            {
                HitEffect.transform.position = transform.position;
                HitEffect.SetActive(true);
            }
            TurnOff();
        }
    }
}
