using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Kakashi_SkillOne : Boss_Skill
{
    Quaternion rotation;
    [SerializeField] GameObject Explosion;
    [SerializeField] public Transform MainPoint;
    public void SetUpDirection(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation = rotation;
        transform.Rotate(0, 0, 90);
    }

    new void OnEnable()
    {
        LifeTime = 3f;
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
                collision.GetComponent<PlayerBase>().TakeDamage(Damage);              
            }
            Explosion.transform.position = MainPoint.position;
            Explosion.SetActive(true);
            TurnOff();
        }
    }
}
