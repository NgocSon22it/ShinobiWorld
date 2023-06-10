using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kakashi : Enemy
{

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        if (photonView.IsMine)
        {
            boss_Entity.ID = "Boss_Kakashi";
            boss_Pool.InitializeProjectilePool("Boss/Arena/Kakashi/");
            boss_Entity = Boss_DAO.GetBossByID(boss_Entity.ID);
            CurrentHealth = boss_Entity.Health;
        }
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


        // Restrict movement to the move area
        clampedPosition = movementBounds.ClosestPoint(transform.position);
        transform.position = new Vector3(clampedPosition.x, clampedPosition.y, transform.position.z);

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
