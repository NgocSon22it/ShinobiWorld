using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;
using TMPro;
using System.Security.Principal;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;
using UnityEngine.Rendering;
using Photon.Realtime;
using System.Data.SqlTypes;

public class Enemy : MonoBehaviourPun, IPunObservable
{
    // Entity
    public Boss_Entity boss_Entity = new Boss_Entity();

    public AreaBoss_Entity areaBoss_Entity = new AreaBoss_Entity();

    //Separate
    public string AreaName;
    public string EnemyID;

    // Move Area
    [SerializeField] public Collider2D movementBounds;
    Vector2 minPosition, maxPosition;
    float randomX, randomY;
    Vector2 randomPosition;

    [SerializeField] GameObject ObjectPool;

    public Vector3 MovePosition;
    public bool isMoving = true;

    public float Break_CurrentTime;
    public float Break_TotalTime = 2f;

    public Vector3 TargetPosition;

    public bool playerInRange = false;
    public Vector3 clampedPosition;
    public float detectionRadius = 5f;

    public float FindTarget_CurrentTime;
    public float FindTarget_TotalTime = 1f;

    public float LocalScaleX;

    // MainPoint
    [SerializeField] Transform MainPoint;

    //Health UI
    [SerializeField] Image CurrentHealth_UI;
    [SerializeField] TMP_Text HealthNumber;

    // Skill Direction
    public Vector2 direction;

    // Health Bar
    [SerializeField] GameObject HealthChakraUI;

    //Component
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigidbody2d;
    public Animator animator;
    public Boss_Pool boss_Pool;

    // Facing
    public bool FacingRight = false;

    public void SetUpComponent()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boss_Pool = GetComponent<Boss_Pool>();
    }

    private void OnEnable()
    {
        SetUpComponent();
    }

    public void SetUp(string EnemyID, string AreaName)
    {
        SetUpComponent();
        boss_Entity = Boss_DAO.GetBossByID(EnemyID);
        areaBoss_Entity = AreaBoss_DAO.GetAreaBossByID(AreaName, EnemyID);

        if (boss_Entity != null && areaBoss_Entity != null)
        {
            SqlDateTime dateTime = new SqlDateTime(System.DateTime.Now);

            if (dateTime >= areaBoss_Entity.TimeSpawn && areaBoss_Entity.isDead == false && areaBoss_Entity.CurrentHealth > 0)
            {
                gameObject.SetActive(true);
                LoadHealthUI();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


    public void Awake()
    {

    }

    public void Start()
    {
        LocalScaleX = transform.localScale.x;
        MovePosition = GetRandomPosition();
        ObjectPool.transform.SetParent(null);
    }

    public void Update()
    {

    }

    public void LoadHealthUI()
    {
        HealthNumber.text = areaBoss_Entity.CurrentHealth + " / " + boss_Entity.Health;
        CurrentHealth_UI.fillAmount = (float)areaBoss_Entity.CurrentHealth / (float)boss_Entity.Health;
    }

    public void TakeDamage(string UserID, int Damage)
    {
        photonView.RPC(nameof(TakeDamageSync), RpcTarget.AllBuffered, UserID, Damage);
    }

    [PunRPC]
    public void TakeDamageSync(string UserID, int Damage)
    {
        areaBoss_Entity.CurrentHealth -= Damage;
        AreaBoss_DAO.UpdateHealthAreaBoss(areaBoss_Entity);

        if (areaBoss_Entity.CurrentHealth <= 0)
        {
            References.AddExperience(boss_Entity.ExpBonus);
            References.AddCoin(boss_Entity.CoinBonus);
            MissionManager.Instance.DoingMission(areaBoss_Entity.BossID);
            AreaBoss_DAO.SetAreaBossDie(areaBoss_Entity.ID, areaBoss_Entity.BossID);
            gameObject.SetActive(false);
        }
        LoadHealthUI();
    }

    public Vector2 GetRandomPosition()
    {
        // Get the bounds of the collider
        minPosition = movementBounds.bounds.min;
        maxPosition = movementBounds.bounds.max;

        // Generate random X and Y coordinates within the collider bounds
        randomX = Random.Range(minPosition.x, maxPosition.x);
        randomY = Random.Range(minPosition.y, maxPosition.y);

        // Create a new position using the random coordinates
        randomPosition = new Vector2(randomX, randomY);

        if (MainPoint.position.x < randomPosition.x && !FacingRight)
        {
            Flip();
        }
        else if (MainPoint.position.x > randomPosition.x && FacingRight)
        {
            Flip();
        }

        return randomPosition;
    }

    public void Flip()
    {
        FacingRight = !FacingRight;
        LocalScaleX *= -1f;


        SetUpFlip(LocalScaleX, 1f, 1f);
    }

    public void SetUpFlip(float x, float y, float z)
    {
        transform.localScale = new Vector3(x, y, z);
        if (HealthChakraUI != null)
        {
            HealthChakraUI.GetComponent<RectTransform>().localScale = new Vector3(x, y, z);
        }
    }

    public void FlipToTarget()
    {
        if (photonView.IsMine)
        {
            if (MainPoint.position.x < TargetPosition.x && !FacingRight)
            {
                Flip();
            }
            else if (MainPoint.position.x > TargetPosition.x && FacingRight)
            {
                Flip();
            }
        }
    }

    public Vector3 FindClostestTarget(float Range, string TargetTag)
    {
        float distanceToClosestTarget = Mathf.Infinity;
        Vector3 closestTargetPosition = Vector3.zero;

        GameObject[] allTarget = GameObject.FindGameObjectsWithTag(TargetTag);


        foreach (GameObject currentTarget in allTarget)
        {
            float distanceToTarget = (currentTarget.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToTarget < distanceToClosestTarget
                && Vector2.Distance(currentTarget.transform.position, transform.position) <= Range
                && currentTarget.GetComponent<BoxCollider2D>().enabled)
            {
                distanceToClosestTarget = distanceToTarget;
                closestTargetPosition = currentTarget.transform.Find("MainPoint").position;

            }
        }

        return closestTargetPosition;
    }

    private void OnDisable()
    {
        areaBoss_Entity.CurrentHealth = boss_Entity.Health;
        areaBoss_Entity.isDead = false;
        AreaBoss_DAO.UpdateHealthAreaBoss(areaBoss_Entity);
        areaBoss_Entity = AreaBoss_DAO.GetAreaBossByID(AreaName, EnemyID);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(areaBoss_Entity.CurrentHealth);
            stream.SendNext(transform.position);

            stream.SendNext(playerInRange);
            stream.SendNext(isMoving);

            stream.SendNext(TargetPosition);

            stream.SendNext(HealthChakraUI.GetComponent<RectTransform>().localScale);


        }
        else
        {
            areaBoss_Entity.CurrentHealth = (int)stream.ReceiveNext();
            MovePosition = (Vector3)stream.ReceiveNext();

            playerInRange = (bool)stream.ReceiveNext();
            isMoving = (bool)stream.ReceiveNext();

            TargetPosition = (Vector3)stream.ReceiveNext();

            HealthChakraUI.GetComponent<RectTransform>().localScale = (Vector3)stream.ReceiveNext();

            LoadHealthUI();
        }
    }
}
