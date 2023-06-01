using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    // Start is called before the first frame update
    new void Start()
    {
        if (photonView.IsMine)
        {
            boss_Entity.ID = "Boss_Bat";
        }

        base.Start();
    }

    new void Update()
    {
        if (photonView.IsMine)
        {
            AttackAndMoveRandom();
        }
    }


    public void Animation_SkillOne()
    {
        if (Target != null)
        {
            GameObject SkillOne = boss_Pool.GetSkillOneFromPool();
            FlipToTarget();
            direction = Target.transform.position - transform.Find("MainPoint").position;

            if (SkillOne != null)
            {
                SkillOne.transform.position = transform.position;
                SkillOne.transform.rotation = transform.rotation;
                SkillOne.GetComponent<Bat_SkillOne>().SetUp(100);
                SkillOne.SetActive(true);
                SkillOne.GetComponent<Rigidbody2D>().velocity = (direction * 3);
            }
        }
    }


}
