using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class Iruka : Enemy
{

    Coroutine AttackCoroutine;
    bool IsStartCoroutine;

    //SkillTwo
    public Vector2 SkillRandomPosition;
    Vector2 Skill_MinPosition, Skill_MaxPosition;
    float X, Y;

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
    }

    new void Update()
    {
        AttackAndMove();

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
            FlipToTarget();
            direction = (TargetPosition - transform.Find("MainPoint").position).normalized;

            if (SkillOne != null)
            {
                SkillOne.transform.position = transform.Find("MainPoint").position;
                SkillOne.transform.rotation = transform.rotation;
                SkillOne.GetComponent<Iruka_SkillOne>().SetUp(100);
                SkillOne.GetComponent<Iruka_SkillOne>().SetUpDirection(direction);
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
            StartCoroutine(SkillTwo_Fire());
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

        TargetPosition = FindClostestTarget(100f, "Player");
        animator.SetTrigger("Skill" + RandomState);
        IsSkilling = true;

        while (IsSkilling)
        {
            yield return null;
        }

        MovePosition = GetRandomPosition();
        isMoving = true;
        IsStartCoroutine = false;
    }

    public Vector2 GetRandomSkillPosition()
    {
        Skill_MinPosition = movementBounds.bounds.min;
        Skill_MaxPosition = movementBounds.bounds.max;

        // Generate random X and Y coordinates within the collider bounds
        X = Random.Range(Skill_MinPosition.x, Skill_MaxPosition.x);
        Y = Random.Range(Skill_MinPosition.y, Skill_MaxPosition.y);

        return new Vector2(X, Y);
    }
    IEnumerator SkillTwo_Fire()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject SkillOne = boss_Pool.GetSkillTwoFromPool();
            if (SkillOne != null)
            {
                SkillRandomPosition = GetRandomSkillPosition();

                SkillOne.transform.position = SkillRandomPosition;
                SkillOne.GetComponent<Iruka_SkillTwo>().SetUp(100);
                SkillOne.SetActive(true);
                yield return new WaitForSeconds(0.3f);
            }
        }

        SetUpSkilling(3f);
    }

    IEnumerator DashNAttack()
    {
        isDashing = true;
        dashTimer = 0f;
        SkillRandomPosition = GetRandomSkillPosition();
        Vector3 direction = (SkillRandomPosition - (Vector2)transform.Find("MainPoint").position).normalized;

        while (dashTimer < dashDuration)
        {
            transform.Translate(direction * dashSpeed * Time.deltaTime);

            dashTimer += Time.deltaTime;

            yield return null;
        }
        isDashing = false;

        TargetPosition = FindClostestTarget(100f, "Player");
        direction = (TargetPosition - transform.Find("MainPoint").position).normalized;

        GameObject center = boss_Pool.GetSkillThreeFromPool();
        if (center != null)
        {
            center.transform.position = transform.Find("MainPoint").position;
            center.transform.rotation = transform.rotation;
            center.GetComponent<Iruka_SkillThree>().SetUp(100);
            center.GetComponent<Iruka_SkillThree>().SetUpDirection(direction, -90);
            center.SetActive(true);
            center.GetComponent<Rigidbody2D>().velocity = direction * 10;
        }

        GameObject left = boss_Pool.GetSkillThreeFromPool();
        if (left != null)
        {
            left.transform.position = transform.Find("MainPoint").position;
            left.transform.rotation = transform.rotation;
            left.GetComponent<Iruka_SkillThree>().SetUp(100);
            left.GetComponent<Iruka_SkillThree>().SetUpDirection(direction, -90 - Angle);
            left.SetActive(true);
            left.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(-Angle, Vector3.forward) * center.GetComponent<Rigidbody2D>().velocity;
        }

        GameObject right = boss_Pool.GetSkillThreeFromPool();
        if (right != null)
        {
            right.transform.position = transform.Find("MainPoint").position;
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

}
