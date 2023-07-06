using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Frog : Enemy
{
    new void Awake()
    {
        EnemyID = "Boss_Frog";
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
        FindTarget_CurrentTime += Time.deltaTime;

        // Check if the interval has passed
        if (FindTarget_CurrentTime >= FindTarget_TotalTime)
        {
            TargetPosition = FindClostestTarget(detectionRadius, "Player");
            FindTarget_CurrentTime = 0f;
        }

        playerInRange = TargetPosition != Vector3.zero;
        // Restrict movement to the move area
        clampedPosition = movementBounds.ClosestPoint(transform.position);
        transform.position = new Vector3(clampedPosition.x, clampedPosition.y, transform.position.z);

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
