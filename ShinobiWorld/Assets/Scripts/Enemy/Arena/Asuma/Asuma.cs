using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asuma : Enemy
{
    Coroutine AttackCoroutine;
    bool IsStartCoroutine;


    //SkillThree
    bool IsSummonClone;
    [SerializeField] List<GameObject> List_SmokeSummon = new List<GameObject>();


    bool IsSkilling;
    int RandomState;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        boss_Health = References.listTrophy.Find(obj => obj.BossID.Equals("Boss_Asuma")).Health;
        CurrentHealth = boss_Health;
        LoadHealthUI(CurrentHealth, boss_Health);
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

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, MovePosition, 3f * Time.deltaTime);

            if (transform.position == MovePosition)
            {
                isMoving = false;
            }

        }
        else
        {
            if (!IsStartCoroutine)
            {
                AttackCoroutine = StartCoroutine(RandomAttack());
            }
        }

        animator.SetBool("Walk", isMoving);
    }

    public void Animation_SkillOne()
    {
        if (TargetPosition != Vector3.negativeInfinity)
        {
            GameObject SkillOne = boss_Pool.GetSkillOneFromPool();

            direction = TargetPosition - transform.Find("MainPoint").position;
            direction.Normalize();

            if (SkillOne != null)
            {
                SkillOne.transform.position = transform.Find("MainPoint").position;
                SkillOne.GetComponent<Asuma_SkillOne>().SetUp(100);
                SkillOne.GetComponent<Asuma_SkillOne>().SetUpDirection(direction);
                SkillOne.SetActive(true);
                SkillOne.GetComponent<Rigidbody2D>().velocity = (direction * 5);
                SetUpSkilling(3f);
            }
        }
    }

    public void Animation_SkillTwo()
    {
        if (TargetPosition != Vector3.negativeInfinity)
        {
            SkillTwo_Fire();
        }
    }

    public void Animation_SkillThree()
    {
        if (TargetPosition != Vector3.negativeInfinity)
        {
            SkillThree_Clone();

        }
    }
    public GameObject GetSmokeSummon()
    {
        for (int i = 0; i < List_SmokeSummon.Count; i++)
        {
            if (!List_SmokeSummon[i].activeInHierarchy)
            {
                return List_SmokeSummon[i];
            }
        }
        return null;
    }
    public IEnumerator RandomAttack()
    {
        IsStartCoroutine = true;

        if (IsSummonClone)
        {
            RandomState = Random.Range(1, 3);
        }
        else
        {
            RandomState = Random.Range(1, 4);
        }

        photonView.RPC(nameof(CallAnimation), RpcTarget.All, "Skill" + RandomState);
        IsSkilling = true;

        while (IsSkilling)
        {
            yield return null;
        }

        MovePosition = GetRandomPosition();
        isMoving = true;
        IsStartCoroutine = false;
    }

    [PunRPC]
    public void CallAnimation(string AnimationName)
    {
        animator.SetTrigger(AnimationName);
    }

    public void SkillThree_Clone()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject SkillThree = boss_Pool.GetSkillThreeFromPool();
            GameObject Smoke = GetSmokeSummon();

            if (SkillThree != null)
            {
                Smoke.transform.position = TargetPosition;
                Smoke.SetActive(true);

                SkillThree.transform.position = TargetPosition;
                SkillThree.SetActive(true);
            }
        }
        IsSummonClone = true;
        SetUpSkilling(3f);

    }
    public void SkillTwo_Fire()
    {

        GameObject SkillTwo = boss_Pool.GetSkillTwoFromPool();
        if (SkillTwo != null)
        {

            SkillTwo.GetComponent<Asuma_SkillTwo>().SetUp(30);
            SkillTwo.transform.position = TargetPosition;
            SkillTwo.SetActive(true);
        }

        SetUpSkilling(3f);

    }
    public void SetUpSkilling(float Seconds)
    {
        StartCoroutine(WaitMomentForSkill(Seconds));
    }
    IEnumerator WaitMomentForSkill(float Seconds)
    {
        yield return new WaitForSeconds(Seconds);
        IsSkilling = false;
    }
}
