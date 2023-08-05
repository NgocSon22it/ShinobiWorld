using Pathfinding;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class Iruka : Enemy
{

    Coroutine AttackCoroutine;
    bool IsStartCoroutine;
    [SerializeField] AIPath aIPath;
    [SerializeField] AIDestinationSetter destinationSetter;
    public float attackRadius;
    public bool CanAttackPlayer;
    GameObject Target;

    //Skill Three
    public float dashSpeed;
    public float dashDuration;

    private float dashTimer = 0f;
    private bool isDashing = false;
    [SerializeField] float Angle = 30f;

    bool IsSkilling;
    int RandomState;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        boss_Health = References.listTrophy.Find(obj => obj.BossID.Equals("Boss_Iruka")).Health;
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
            CanAttackPlayer = Physics2D.OverlapCircle(transform.position, attackRadius, AttackableLayer);
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

            direction = TargetPosition - transform.Find("FirePoint").position;
            direction.Normalize();

            if (SkillOne != null)
            {
                SkillOne.transform.position = transform.Find("FirePoint").position;
                SkillOne.GetComponent<Iruka_SkillOne>().SetUp(100);
                SkillOne.GetComponent<Iruka_SkillOne>().SetUpDirection(direction);
                SkillOne.SetActive(true);
                SkillOne.GetComponent<Rigidbody2D>().velocity = (direction * 10);
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
        if (TargetPosition != Vector3.negativeInfinity && !isDashing)
        {
            StartCoroutine(DashNAttack());
        }

    }

    public IEnumerator RandomAttack()
    {
        IsStartCoroutine = true;

        RandomState = Random.Range(1, 4);

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

    [PunRPC]
    public void CallAnimation(string AnimationName)
    {
        animator.SetTrigger(AnimationName);
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

    public void SkillTwo_Fire()
    {
        GameObject SkillOne = boss_Pool.GetSkillTwoFromPool();
        if (SkillOne != null)
        {
            SkillOne.transform.position = TargetPosition;
            SkillOne.GetComponent<Iruka_SkillTwo>().SetUp(100);
            SkillOne.SetActive(true);

        }
        SetUpSkilling(3f);
    }

    IEnumerator DashNAttack()
    {
        isDashing = true;
        dashTimer = 0f;
        Vector3 direction = (TargetPosition - transform.Find("MainPoint").position).normalized;

        while (dashTimer < dashDuration)
        {
            transform.Translate(direction * dashSpeed * Time.deltaTime);

            dashTimer += Time.deltaTime;

            yield return null;
        }
        isDashing = false;

        direction = (TargetPosition - transform.Find("FirePoint").position).normalized;

        GameObject center = boss_Pool.GetSkillThreeFromPool();
        if (center != null)
        {
            center.transform.position = transform.Find("FirePoint").position;
            center.transform.rotation = transform.rotation;
            center.GetComponent<Iruka_SkillThree>().SetUp(100);
            center.GetComponent<Iruka_SkillThree>().SetUpDirection(direction, -90);
            center.SetActive(true);
            center.GetComponent<Rigidbody2D>().velocity = direction * 10;
        }

        GameObject left = boss_Pool.GetSkillThreeFromPool();
        if (left != null)
        {
            left.transform.position = transform.Find("FirePoint").position;
            left.transform.rotation = transform.rotation;
            left.GetComponent<Iruka_SkillThree>().SetUp(100);
            left.GetComponent<Iruka_SkillThree>().SetUpDirection(direction, -90 - Angle);
            left.SetActive(true);
            left.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(-Angle, Vector3.forward) * center.GetComponent<Rigidbody2D>().velocity;
        }

        GameObject right = boss_Pool.GetSkillThreeFromPool();
        if (right != null)
        {
            right.transform.position = transform.Find("FirePoint").position;
            right.transform.rotation = transform.rotation;
            right.GetComponent<Iruka_SkillThree>().SetUp(100);
            right.GetComponent<Iruka_SkillThree>().SetUpDirection(direction, -90 + Angle);
            right.SetActive(true);
            right.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(Angle, Vector3.forward) * center.GetComponent<Rigidbody2D>().velocity;
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
