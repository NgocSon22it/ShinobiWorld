using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Kakashi : Enemy
{

    Coroutine AttackCoroutine;
    bool IsStartCoroutine;

    //Skill Three
    public GameObject ChidoriPrefabs;
    public float dashSpeed;
    public float dashDuration;

    private float dashTimer = 0f;
    private bool isDashing = false;


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
                SkillOne.GetComponent<Kakashi_SkillOne>().SetUp(100);
                SkillOne.SetActive(true);
                SkillOne.GetComponent<Rigidbody2D>().velocity = (direction * 5);
            }
        }
    }

    public void Animation_SkillTwo()
    {
        if (TargetPosition != Vector3.negativeInfinity)
        {
            GameObject SkillOne = boss_Pool.GetSkillOneFromPool();
            FlipToTarget();
            direction = (TargetPosition - transform.Find("MainPoint").position).normalized;

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

    public void Animation_SkillThree()
    {
        if (TargetPosition != Vector3.negativeInfinity && !isDashing)
        {
            StartCoroutine(Chidori());
        }
        
    }

    public IEnumerator RandomAttack()
    {
        IsStartCoroutine = true;
        int RandomState = Random.Range(1, 4);

        TargetPosition = FindClostestTarget(100f, "Player");
        animator.SetTrigger("Skill" + RandomState);

        yield return new WaitForSeconds(3f);

        MovePosition = GetRandomPosition();
        isMoving = true;
        IsStartCoroutine = false;
    }

    IEnumerator Chidori()
    {
        isDashing = true;
        ChidoriPrefabs.SetActive(true);
        dashTimer = 0f;

        Vector3 direction = (TargetPosition - transform.Find("MainPoint").position).normalized;

        while (dashTimer < dashDuration)
        {
            // Move the player towards the target at the dash speed
            transform.Translate(direction * dashSpeed * Time.deltaTime);

            // Update the dash timer
            dashTimer += Time.deltaTime;

            yield return null;
        }

        ChidoriPrefabs.SetActive(false);
        isDashing = false;
    }


}
