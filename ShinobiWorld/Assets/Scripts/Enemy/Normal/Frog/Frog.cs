using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Frog : Enemy
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
        playerInRange = CheckPlayerInRange();

        animator.SetBool("PlayerInRange", playerInRange);
    }

    public void Animation_SkillOne()
    {
        if (TargetPosition != Vector3.zero)
        {
            GameObject SkillOne = boss_Pool.GetSkillOneFromPool();

            if (SkillOne != null)
            {
                SkillOne.transform.position = TargetPosition;
                SkillOne.GetComponent<Frog_Attack>().SetUp(100);
                SkillOne.GetComponent<Frog_Attack>().SetUpPoint(transform.position, TargetPosition);
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
