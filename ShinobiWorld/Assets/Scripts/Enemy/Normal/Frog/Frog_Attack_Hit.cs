using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog_Attack_Hit : Boss_Skill
{
    [SerializeField] Collider2D collider2d;
    new void OnEnable()
    {
        LifeTime = 1f;
        base.OnEnable();
        collider2d.enabled = true;
        StartCoroutine(TurnOffColliderCoroutine());
    }

    new void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    IEnumerator TurnOffColliderCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        collider2d.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (AttackAble_Tag.Contains(collision.gameObject.tag))
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.GetComponent<PlayerBase>().TakeDamage(Damage);
            }
        }
    }
}
