using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Kakashi : Enemy
{

    Coroutine AttackCoroutine;
    bool IsStartCoroutine;

    [SerializeField] List<GameObject> List_Electric = new List<GameObject>();
    [SerializeField] List<GameObject> List_Fire = new List<GameObject>();

    //Skill Three
    public GameObject ChidoriPrefabs;
    public float dashSpeed;
    public float dashDuration;

    private float dashTimer = 0f;
    private bool isDashing = false;

    bool IsSkilling;
    int RandomState;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    new void FixedUpdate()
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
                SkillOne.GetComponent<Kakashi_SkillOne>().SetUpDirection(direction);
                SkillOne.SetActive(true);
                SkillOne.GetComponent<Rigidbody2D>().velocity = (direction * 8);
                SetUpSkilling(3f);
            }
        }
    }

    public void Animation_SkillTwo()
    {
        if (TargetPosition != Vector3.negativeInfinity)
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                SkillTwo_Fire();
            }
            else
            {
                SkillTwo_Electric();
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

    IEnumerator Chidori()
    {
        isDashing = true;
        ChidoriPrefabs.GetComponent<Kakashi_SkillThree>().SetUp(100);
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

        SetUpSkilling(3f);
    }

    public void SkillTwo_Electric()
    {
        GameObject SkillOne = GetElectric();
        if (SkillOne != null)
        {
            SkillOne.GetComponent<Kakashi_SkillTwo_Electric>().SetUp(100);
            SkillOne.transform.position = TargetPosition;
            SkillOne.SetActive(true);
        }

        SetUpSkilling(3f);

    }
    public void SkillTwo_Fire()
    {

        GameObject SkillOne = GetFire();
        if (SkillOne != null)
        {
            SkillOne.transform.position = TargetPosition;
            SkillOne.GetComponent<Kakashi_SkillTwo_Fire>().SetUpPoint(TargetPosition);
            SkillOne.GetComponent<Kakashi_SkillTwo_Fire>().SetUp(100);
            SkillOne.SetActive(true);
        }

        SetUpSkilling(3f);
    }
    public GameObject GetElectric()
    {
        for (int i = 0; i < List_Electric.Count; i++)
        {
            if (!List_Electric[i].activeInHierarchy)
            {
                return List_Electric[i];
            }
        }
        return null;
    }
    public GameObject GetFire()
    {
        for (int i = 0; i < List_Fire.Count; i++)
        {
            if (!List_Fire[i].activeInHierarchy)
            {
                return List_Fire[i];
            }
        }
        return null;
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
