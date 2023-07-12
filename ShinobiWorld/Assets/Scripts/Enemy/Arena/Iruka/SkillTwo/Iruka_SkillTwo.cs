using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iruka_SkillTwo : Boss_Skill
{
    [SerializeField] GameObject MainFire;
    [SerializeField] CircleCollider2D CircleCol;

    Coroutine Fire;

    new void OnEnable()
    {
        LifeTime = 1.8f;
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
        SetUpDamage(false);
    }

    private IEnumerator StartDamage()
    {
        yield return new WaitForSeconds(1f);
        SetUpDamage(true);
    }

    public void SetUpDamage(bool status)
    {
        MainFire.SetActive(status);
        CircleCol.enabled = status;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (AttackAble_Tag.Contains(collision.gameObject.tag))
        {
            if (collision.gameObject.tag == "Player")
            {
                //collision.GetComponent<PlayerBase>().TakeDamage(Damage);
            }
        }
    }
}
