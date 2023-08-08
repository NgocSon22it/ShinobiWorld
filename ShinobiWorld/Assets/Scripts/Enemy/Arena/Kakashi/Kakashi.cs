using Pathfinding;
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

    [SerializeField] AIPath aIPath;
    [SerializeField] AIDestinationSetter destinationSetter;
    public float attackRadius;
    public bool CanAttackPlayer;
    GameObject Target;
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
        SetUpHealth();
        Target = FindClostestTargetToFollow(detectionRadius, "Player");
        destinationSetter.target = Target.transform;
        LoadHealthUI(CurrentHealth, boss_Health);
    }
    public void SetUpHealth()
    {
        boss_Health = References.listTrophy.Find(obj => obj.BossID.Equals("Boss_Kakashi")).Health * BossArena_Manager.Instance.GetNumberPlayer();
        CurrentHealth = boss_Health;
    }
    new void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, MovePosition, Time.deltaTime * 1f);
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
                SkillOne.GetComponent<Kakashi_SkillOne>().SetUp(550);
                SkillOne.GetComponent<Kakashi_SkillOne>().SetUpDirection(direction);
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
            SkillTwo_Electric();
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

    IEnumerator Chidori()
    {
        isDashing = true;
        ChidoriPrefabs.GetComponent<Kakashi_SkillThree>().SetUp(500);
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
        GameObject SkillTwo = GetElectric();
        if (SkillTwo != null)
        {
            SkillTwo.GetComponent<Kakashi_SkillTwo_Electric>().SetUp(500);
            SkillTwo.transform.position = TargetPosition;
            SkillTwo.SetActive(true);
        }

        SetUpSkilling(3f);
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
