using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asuma_SkillTwo : Boss_Skill
{
    [SerializeField] GameObject MainFire;
    [SerializeField] CircleCollider2D Col;

    Coroutine Fire;
    Coroutine FireDamage;

    new void OnEnable()
    {
        LifeTime = 6.5f;
        Fire = StartCoroutine(StartDamage());
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
        if (Fire != null)
        {
            StopCoroutine(Fire);
        }
        if (FireDamage != null)
        {
            StopCoroutine(FireDamage);
        }
        SetUpDamage(false);
    }

    private IEnumerator StartDamage()
    {
        yield return new WaitForSeconds(1f);
        SetUpDamage(true);
        FireDamage = StartCoroutine(LogTriggeredObjects());
    }

    public void SetUpDamage(bool status)
    {
        MainFire.SetActive(status);
        Col.enabled = status;
    }

    private IEnumerator LogTriggeredObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            List<Collider2D> colliders = new List<Collider2D>();
            Physics2D.OverlapCollider(Col, new ContactFilter2D(), colliders);

            foreach (Collider2D collider in colliders)
            {
                if (AttackAble_Tag.Contains(collider.gameObject.tag))
                {
                    if (collider.CompareTag("Player"))
                    {
                        collider.GetComponent<PlayerBase>().TakeDamage(Damage);
                    }
                }
            }
        }
    }
}
