using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Frog : Enemy
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();


        LoadHealthUI();
    }

    new void Update()
    {
        if (photonView.IsMine)
        {
            Attack();
        }
    }
    public void Attack()
    {      
        FindTarget_CurrentTime += Time.deltaTime;

        // Check if the interval has passed
        if (FindTarget_CurrentTime >= FindTarget_TotalTime)
        {
            TargetPosition = FindClostestTarget(detectionRadius, "Player");
            // Call the RPC and reset the timer
            FindTarget_CurrentTime = 0f;
        }

        playerInRange = TargetPosition != Vector3.zero;

        animator.SetBool("Attack", playerInRange);
    }


    public void Animation_SkillOne()
    {
        if (TargetPosition != Vector3.zero)
        {
            GameObject SkillOne = boss_Pool.GetSkillOneFromPool();
            FlipToTarget();
            direction = TargetPosition - transform.Find("MainPoint").position;

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
