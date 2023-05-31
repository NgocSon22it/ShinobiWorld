using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class JudgmentJustice : PlayerSkill
{
    public Vector3  EndPoint;

    public GameObject Explosion;
    public void SetUpPoint(Vector3 EndPoint)
    {
        this.EndPoint = EndPoint;
    }

    private void Update()
    {
        if (EndPoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, EndPoint, 8 * Time.deltaTime);
        }

        if(transform.position == EndPoint)
        {
           GameObject a =  Instantiate(Explosion, transform.position, Quaternion.identity);
           Destroy(a, 2f);
           TurnOff();
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
            if (collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<Enemy>().TakeDamage(UserID, Damage);
            }
        }
    }
}
