using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (photonView.IsMine)
        {
            boss_Entity.ID = "Boss_Bat";
            boss_Pool.InitializeProjectilePool("Boss/Normal/Frog/");
            boss_Entity = Boss_DAO.GetBossByID(boss_Entity.ID);
            CurrentHealth = boss_Entity.Health;
            MovePosition = GetRandomPosition();
        }

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

        animator.SetBool("Attack", playerInRange);
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
                SkillOne.GetComponent<Frog_SkillOne>().SetUp(100);
                SkillOne.SetActive(true);
                SkillOne.GetComponent<Rigidbody2D>().velocity = (direction * 3);
            }
        }
    }
}
