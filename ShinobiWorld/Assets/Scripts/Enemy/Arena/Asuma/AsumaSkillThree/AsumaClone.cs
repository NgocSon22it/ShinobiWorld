using Assets.Scripts.Database.Entity;
using Pathfinding;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsumaClone : Enemy
{
    Coroutine AttackCoroutine;
    bool IsStartCoroutine;
    bool IsSkilling;

    [SerializeField] AIPath aIPath;
    [SerializeField] AIDestinationSetter destinationSetter;
    public float attackRadius;
    public bool CanAttackPlayer;
    GameObject Target;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        boss_Health = References.listTrophy.Find(obj => obj.BossID.Equals("Boss_Asuma")).Health;
        boss_Health /= 2;
        CurrentHealth = boss_Health;
        Target = FindClostestTargetToFollow(detectionRadius, "Player");
        destinationSetter.target = Target.transform;
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
            MovePosition = aIPath.desiredVelocity;
            CanAttackPlayer = Physics2D.OverlapCircle(MainPoint.position, attackRadius, AttackableLayer);
            if (CanAttackPlayer)
            {
                aIPath.canMove = false;
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

    [PunRPC]
    public void CallAnimation(string AnimationName)
    {
        animator.SetTrigger(AnimationName);
    }

    public IEnumerator RandomAttack()
    {
        IsStartCoroutine = true;
        int RandomState = Random.Range(1, 3);

        photonView.RPC(nameof(CallAnimation), RpcTarget.All, "Skill" + RandomState);

        IsSkilling = true;

        while (IsSkilling)
        {
            yield return null;
        }

        Target = FindClostestTargetToFollow(detectionRadius, "Player");
        destinationSetter.target = Target.transform;
        aIPath.canMove = true;
        isMoving = true;
        IsStartCoroutine = false;
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

    public GameObject FindClostestTargetToFollow(float Range, string TargetTag)
    {
        float distanceToClosestTarget = Mathf.Infinity;
        GameObject closestTargetPosition = null;

        GameObject[] allTarget = GameObject.FindGameObjectsWithTag(TargetTag);

        foreach (GameObject currentTarget in allTarget)
        {
            float distanceToTarget = (currentTarget.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToTarget < distanceToClosestTarget
                && Vector2.Distance(currentTarget.transform.position, transform.position) <= Range
                && currentTarget.GetComponent<BoxCollider2D>().enabled)
            {
                distanceToClosestTarget = distanceToTarget;
                closestTargetPosition = currentTarget;

            }
        }

        return closestTargetPosition;
    }
    public void FollowPlayer()
    {
        if (MainPoint.position.x < Target.transform.position.x && !FacingRight)
        {
            Flip();
        }
        else if (MainPoint.position.x > Target.transform.position.x && FacingRight)
        {
            Flip();
        }
    }
    IEnumerator WaitMomentForSkill(float Seconds)
    {
        yield return new WaitForSeconds(Seconds);
        IsSkilling = false;
    }
}
