using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Locusts : Enemy
{
    [SerializeField] List<GameObject> ListAttack_Hit = new List<GameObject>();

    new void Awake()
    {
        SetUp(EnemyID, AreaID);
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

        playerInRange = CheckPlayerInRange();

        animator.SetBool("PlayerInRange", playerInRange);
        animator.SetBool("Walk", isMoving);

    }

    public void Animation_SkillOne()
    {
        if (TargetPosition != Vector3.zero)
        {
            GameObject SkillOne = boss_Pool.GetSkillOneFromPool();

            if (SkillOne != null)
            {
                SkillOne.transform.position = TargetPosition;
                SkillOne.GetComponent<Locusts_Attack>().SetUp(100);
                SkillOne.GetComponent<Locusts_Attack>().SetUpPoint(transform.position, TargetPosition);
                SkillOne.SetActive(true);
            }
        }
    }

    public GameObject GetAttack_Hit()
    {
        for (int i = 0; i < ListAttack_Hit.Count; i++)
        {
            if (!ListAttack_Hit[i].activeInHierarchy)
            {
                return ListAttack_Hit[i];
            }
        }
        return null;
    }
}
