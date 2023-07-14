using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kakashi_SkillTwo_Electric : Boss_Skill
{
    [SerializeField] GameObject MainElectric;
    [SerializeField] CircleCollider2D CircleCol;

    Coroutine Electric;

    new void OnEnable()
    {
        LifeTime = 2.5f;
        Electric = StartCoroutine(StartDamage());
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
        if (Electric != null)
        {
            StopCoroutine(Electric);
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
        MainElectric.SetActive(status);
        CircleCol.enabled = status;
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
