using Assets.Scripts.Database.Entity;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class Melee_SkillTwo : PlayerSkill
{
    [SerializeField] Collider2D collider2;
    [SerializeField] Transform PlayerTransform;

    float DamageSeconds = 0.2f;
    Coroutine FlyCoroutine;


    new void OnEnable()
    {
        LifeTime = 5f;
        FlyCoroutine = StartCoroutine(LogTriggeredObjects());
        base.OnEnable();
    }

    private void FixedUpdate()
    {
        transform.position = PlayerTransform.position;
    }


    new void OnDisable()
    {
        base.OnDisable();
        StopCoroutine(FlyCoroutine);
    }

    private IEnumerator LogTriggeredObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(DamageSeconds);
            List<Collider2D> colliders = new List<Collider2D>();
            Physics2D.OverlapCollider(collider2, new ContactFilter2D(), colliders);

            foreach (Collider2D collider in colliders)
            {
                if (AttackAble_Tag.Contains(collider.gameObject.tag))
                {
                    if (collider.CompareTag("Enemy") || collider.gameObject.tag == "Clone")
                    {
                        collider.GetComponent<Enemy>().TakeDamage(PV.ViewID, Damage * 10);
                    }                 
                }
                if (collider.CompareTag("Player")
                        && collider.gameObject.GetComponent<PlayerBase>().accountStatus == AccountStatus.PK
                        && collider.gameObject.GetComponent<PhotonView>() != PV)
                {
                    collider.GetComponent<PlayerBase>().TakeDamage(Damage);
                }
            }
        }
    }
}
