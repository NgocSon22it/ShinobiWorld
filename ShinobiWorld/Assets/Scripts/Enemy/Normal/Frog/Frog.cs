using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Frog : Enemy
{
    new void Awake()
    {
        EnemyID = "Frog";
        SetUp(EnemyID, AreaName);
    }
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }
    new void Update()
    {
        base.Update();
        Attack();
    }

    public void Attack()
    {
        

        animator.SetBool("PlayerInRange", playerInRange);
    }


    public void Animation_SkillOne()
    {
        if (TargetPosition != Vector3.negativeInfinity)
        {
            GameObject SkillOne = boss_Pool.GetSkillOneFromPool();
            FlipToTarget();
            direction = (TargetPosition - transform.Find("MainPoint").position).normalized;


            if (SkillOne != null)
            {
                SkillOne.transform.position = transform.position;
                SkillOne.transform.rotation = transform.rotation;
                SkillOne.GetComponent<Frog_SkillOne>().SetUp(100);
                SkillOne.SetActive(true);
                SkillOne.GetComponent<Rigidbody2D>().velocity = (direction * 3);
            }
        }
    }
}
