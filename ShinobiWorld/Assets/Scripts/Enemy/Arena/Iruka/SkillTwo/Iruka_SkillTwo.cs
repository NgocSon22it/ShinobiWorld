using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iruka_SkillTwo : Boss_Skill
{
    [SerializeField] GameObject MainFire;
    [SerializeField] CircleCollider2D CircleCol;

    Coroutine Electric;

    new void OnEnable()
    {
        LifeTime = 1.8f;
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
