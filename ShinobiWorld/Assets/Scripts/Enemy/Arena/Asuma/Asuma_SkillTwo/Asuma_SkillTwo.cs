using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asuma_SkillTwo : Boss_Skill
{
    [SerializeField] GameObject MainFire;
    [SerializeField] CircleCollider2D Col;

    Coroutine Electric;

    new void OnEnable()
    {
        LifeTime = 6.5f;
        Electric = StartCoroutine(StartDamage());
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
        StopCoroutine(Electric);
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
        Col.enabled = status;
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
