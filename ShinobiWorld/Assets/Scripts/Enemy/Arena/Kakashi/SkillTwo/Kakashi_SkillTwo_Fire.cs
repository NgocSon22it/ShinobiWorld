using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class Kakashi_SkillTwo_Fire : Boss_Skill
{
    [SerializeField] GameObject MainFire;
    [SerializeField] GameObject MainFireExplosion;

    public Vector3 EndPoint;

    public void SetUpPoint(Vector3 EndPoint)
    {
        this.EndPoint = EndPoint;
        MainFire.transform.position = EndPoint + new Vector3(0, 8, 0);
    }

    private void Update()
    {
        if (EndPoint != null)
        {
            MainFire.transform.position = Vector3.MoveTowards(MainFire.transform.position, EndPoint, 5 * Time.deltaTime);
        }

        if (MainFire.transform.position == EndPoint)
        {
            TurnOff();
            MainFireExplosion.transform.position = EndPoint;
            MainFireExplosion.SetActive(true);
        }
    }
    new void OnEnable()
    {
        LifeTime = 5f;
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
            if (collision.gameObject.tag == "Player")
            {
                //collision.GetComponent<PlayerBase>().TakeDamage(Damage);
            }
        }
    }
}
