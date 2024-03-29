using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squid : Enemy
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
    new void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, MovePosition, Time.deltaTime * lerpFactor);
        }
        else
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
                SkillOne.GetComponent<Squid_Attack>().SetUp(300);
                SkillOne.GetComponent<Squid_Attack>().SetUpPoint(FirePoint.position, TargetPosition);
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
