using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    new void Awake()
    {
        EnemyID = "Boss_Bat";
        AreaName = "LL1_Bat1";
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
        AttackAndMove();
    }

    public void AttackAndMove()
    {
        if (playerInRange)
        {
            // Stop moving
            isMoving = false;
        }
        else
        {
            if (isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, MovePosition, 3f * Time.deltaTime);

                if (transform.position == MovePosition)
                {
                    Break_CurrentTime = Break_TotalTime;
                    isMoving = false;
                }

            }
            else
            {
                Break_CurrentTime -= Time.deltaTime;

                if (Break_CurrentTime <= 0f)
                {
                    MovePosition = GetRandomPosition();
                    isMoving = true;
                }
            }
        }
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
        animator.SetBool("Walk", isMoving);

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
                SkillOne.GetComponent<Bat_SkillOne>().SetUp(100);
                SkillOne.SetActive(true);
                SkillOne.GetComponent<Rigidbody2D>().velocity = (direction * 3);
            }
        }
    }




}
