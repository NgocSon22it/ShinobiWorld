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
    public Enemy_Entity enemy_Entity = new Enemy_Entity();
    public Boss_Entity boss_Entity = new Boss_Entity();
    public AreaEnemy_Entity AreaEnemy_Entity = new AreaEnemy_Entity();

    //Separate
    public string AreaName = "";
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
    public float detectionRadius = 5f;
    public LayerMask AttackableLayer;

    public float LocalScaleX;

    //BossType
    [SerializeField] protected BossType BossType;
    public int CurrentHealth;

    // MainPoint
    [SerializeField] Transform MainPoint;

    //Health UI
    [SerializeField] Image CurrentHealth_UI;
    [SerializeField] TMP_Text HealthNumber;

    [SerializeField] GameObject DeathEffect;

    // Skill Direction
    public Vector2 direction;

    // Health Bar
    [SerializeField] GameObject HealthChakraUI;

    //Component
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigidbody2d;
    public Animator animator;
    public Boss_Pool boss_Pool;
    public Collider2D collider2d;

    // Facing
    public bool FacingRight = false;

    public void SetUp(string EnemyID, string AreaName)
    {
        if (!string.IsNullOrEmpty(AreaName))
        {
            enemy_Entity = Enemy_DAO.GetEnemyByID(EnemyID);
            AreaEnemy_Entity = AreaEnemy_DAO.GetAreaEnemyByID(AreaName, EnemyID);

            if (enemy_Entity != null && AreaEnemy_Entity != null)
            {
                SqlDateTime dateTime = new SqlDateTime(System.DateTime.Now);

                if (dateTime >= AreaEnemy_Entity.TimeSpawn && AreaEnemy_Entity.IsDead == false && AreaEnemy_Entity.CurrentHealth > 0)
                {
                    gameObject.SetActive(true);
                    LoadHealthUI(AreaEnemy_Entity.CurrentHealth, enemy_Entity.Health);
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
        if (ObjectPool != null)
        {
            ObjectPool.transform.SetParent(null);
        }
    }

    public void Update()
    {

    }

    public void LoadHealthUI(float CurrentHealth, float TotalHealth)
    {
        HealthNumber.text = CurrentHealth + " / " + TotalHealth;
        CurrentHealth_UI.fillAmount = CurrentHealth / TotalHealth;
    }

    public void TakeDamage(string UserID, int Damage)
    {
        switch (BossType)
        {
            case BossType.BossType_Normal:
                photonView.RPC(nameof(TakeDamageSync), RpcTarget.AllBuffered, UserID, Damage);
                break;

            case BossType.BossType_Arena:
                TakeDamage_Arena(Damage);
                break;
        }

    }

    public void TakeDamage_Arena(int Damage)
    {
        CurrentHealth -= Damage;
        LoadHealthUI(CurrentHealth, enemy_Entity.Health);
        switch (gameObject.tag)
        {
            case "Enemy":
                if (CurrentHealth <= 0)
                {
                    Disappear();
                    BossArena_Manager.Instance.Battle_End(true);
                }
                break;

            case "Clone":
                if (CurrentHealth <= 0)
                {
                    Disappear();
                }
                break;
        }

    }

    [PunRPC]
    public void TakeDamageSync(string UserID, int Damage)
    {
        AreaEnemy_Entity.CurrentHealth -= Damage;
        AreaEnemy_DAO.UpdateHealthAreaEnemy(AreaEnemy_Entity);

        if (AreaEnemy_Entity.CurrentHealth <= 0)
        {
            References.AddExperience(enemy_Entity.ExpBonus);
            References.AddCoin(enemy_Entity.CoinBonus);

            MissionManager.Instance.DoingMission(AreaEnemy_Entity.EnemyID);

            AreaEnemy_DAO.SetAreaEnemyDie(AreaEnemy_Entity.ID, AreaEnemy_Entity.EnemyID);
            gameObject.SetActive(false);
            Disappear();
        }

        LoadHealthUI(AreaEnemy_Entity.CurrentHealth, enemy_Entity.Health);
    }

    public void Disappear()
    {
        switch (BossType)
        {
            case BossType.BossType_Normal:
                AreaEnemy_Entity.CurrentHealth = enemy_Entity.Health;
                AreaEnemy_Entity.IsDead = false;
                AreaEnemy_DAO.UpdateHealthAreaEnemy(AreaEnemy_Entity);
                AreaEnemy_Entity = AreaEnemy_DAO.GetAreaEnemyByID(AreaName, EnemyID);
                break;

            case BossType.BossType_Arena:
                gameObject.SetActive(false);              
                break;
        }
        DeathEffect.transform.position = transform.position;
        DeathEffect.SetActive(true);
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
        TargetPosition = FindClostestTarget(detectionRadius + 1, "Player");

        if (MainPoint.position.x < TargetPosition.x && !FacingRight)
        {
            Flip();
        }
        else if (MainPoint.position.x > TargetPosition.x && FacingRight)
        {
            Flip();
        }
    }

    public bool CheckPlayerInRange()
    {
        return Physics2D.OverlapCircle(MainPoint.position, detectionRadius, AttackableLayer);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (BossType == BossType.BossType_Normal)
            {
                stream.SendNext(AreaEnemy_Entity.CurrentHealth);
                stream.SendNext(transform.position);

                stream.SendNext(playerInRange);
                stream.SendNext(isMoving);

                stream.SendNext(TargetPosition);

                stream.SendNext(HealthChakraUI.GetComponent<RectTransform>().localScale);
            }



        }
        else
        {
            if (BossType == BossType.BossType_Normal)
            {
                AreaEnemy_Entity.CurrentHealth = (int)stream.ReceiveNext();
                LoadHealthUI(AreaEnemy_Entity.CurrentHealth, enemy_Entity.Health);
                MovePosition = (Vector3)stream.ReceiveNext();

                playerInRange = (bool)stream.ReceiveNext();
                isMoving = (bool)stream.ReceiveNext();

                TargetPosition = (Vector3)stream.ReceiveNext();

                HealthChakraUI.GetComponent<RectTransform>().localScale = (Vector3)stream.ReceiveNext();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(MainPoint.position, detectionRadius);

    }
}
