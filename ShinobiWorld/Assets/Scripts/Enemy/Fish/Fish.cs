using Assets.Scripts.Database.Entity;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Enemy
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (photonView.IsMine)
        {
            FacingRight = false;
            boss_Entity.ID = "Boss_Bat";
            boss_Pool.InitializeProjectilePool("Boss/Fish/");
            boss_Entity = Boss_DAO.GetBossByID(boss_Entity.ID);
            CurrentHealth = boss_Entity.Health;
        }

        LoadHealthUI();
    }

    new void Update()
    {
        if (photonView.IsMine)
        {
            AttackAndMove();
        }
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
            if (FindClostestTarget(detectionRadius, "Player") != null)
            {
                photonView.RPC(nameof(SyncFindTarget), RpcTarget.AllBuffered);
            }
            else
            {
                Target = null;
                Debug.Log("Reduce");
            }
            // Call the RPC and reset the timer
            FindTarget_CurrentTime = 0f;
        }

        // Update the player in range status
        playerInRange = Target != null;

        // Restrict movement to the move area
        clampedPosition = movementBounds.ClosestPoint(transform.position);
        transform.position = new Vector3(clampedPosition.x, clampedPosition.y, transform.position.z);

        animator.SetBool("Attack", playerInRange);
        animator.SetBool("Walk", isMoving);
    }


    public void Animation_SkillOne()
    {
        if (Target != null)
        {
            GameObject SkillOne = boss_Pool.GetSkillOneFromPool();
            FlipToTarget();
            direction = Target.transform.Find("MainPoint").position - transform.Find("MainPoint").position;

            if (SkillOne != null)
            {
                SkillOne.transform.position = transform.position;
                SkillOne.transform.rotation = transform.rotation;
                SkillOne.GetComponent<Fish_SkillOne>().SetUp(100);
                SkillOne.SetActive(true);
                SkillOne.GetComponent<Rigidbody2D>().velocity = (direction * 3);
            }
        }
    }




}
