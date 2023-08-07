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
using ExitGames.Client.Photon;

public class Enemy : MonoBehaviourPun, IPunObservable
{
    // Entity
    public Enemy_Entity enemy_Entity = new Enemy_Entity();
    protected int boss_Health;
    public AreaEnemy_Entity AreaEnemy_Entity = new AreaEnemy_Entity();

    //Separate
    public string AreaID = "";
    public string EnemyID;
    public Coroutine SpawnEnemyCoroutine;

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
    public float detectionRadius;
    public LayerMask AttackableLayer;

    public float LocalScaleX;

    //BossType
    [SerializeField] protected BossType BossType;
    public int CurrentHealth;

    // MainPoint
    [SerializeField] protected Transform MainPoint;
    [SerializeField] protected Transform FirePoint;

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

    // Lag Reduce
    protected Vector3 networkPosition;
    protected float lerpFactor = 3f;

    public void SetUp(string EnemyID, string AreaID)
    {
        if (!string.IsNullOrEmpty(EnemyID) && !string.IsNullOrEmpty(AreaID))
        {
            enemy_Entity = Enemy_DAO.GetEnemyByID(EnemyID);
            AreaEnemy_Entity = AreaEnemy_DAO.GetAreaEnemyByID(AreaID, EnemyID);

            if (enemy_Entity != null && AreaEnemy_Entity != null)
            {
                SqlDateTime dateTime = new SqlDateTime(System.DateTime.Now);
                if (AreaEnemy_Entity.TimeSpawn <= dateTime) { AreaEnemy_Entity.IsDead = false; AreaEnemy_DAO.SetAreaEnemyAlive(AreaID, EnemyID); }

                if (dateTime >= AreaEnemy_Entity.TimeSpawn && AreaEnemy_Entity.IsDead == false)
                {
                    CurrentHealth = enemy_Entity.Health;
                    LoadHealthUI(CurrentHealth, enemy_Entity.Health);
                    gameObject.SetActive(true);
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
        if (movementBounds != null) { MovePosition = GetRandomPosition(); }
        if (ObjectPool != null) { ObjectPool.transform.SetParent(null); }
    }

    public void FixedUpdate()
    {

    }


    public void LoadHealthUI(float CurrentHealth, float TotalHealth)
    {
        HealthNumber.text = CurrentHealth + " / " + TotalHealth;
        CurrentHealth_UI.fillAmount = CurrentHealth / TotalHealth;
    }

    public void TakeDamage(int UserID, int Damage)
    {
        switch (BossType)
        {
            case BossType.BossType_Normal:
                TakeDamageSync(UserID, Damage);
                break;

            case BossType.BossType_Arena:
                TakeDamage_Arena(Damage);
                break;
        }

    }

    public void TakeDamage_Arena(int Damage)
    {
        CurrentHealth -= Damage;
        LoadHealthUI(CurrentHealth, boss_Health);

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

    public void TakeDamageSync(int UserID, int Damage)
    {
        CurrentHealth -= Damage;

        if (CurrentHealth <= 0)
        {
            GameObject LastHitPlayer = PhotonView.Find(UserID).gameObject;

            if (LastHitPlayer != null)
            {
                PlayerBase playerBase = LastHitPlayer.GetComponent<PlayerBase>();

                if (References.accountRefer.ID == playerBase.AccountEntity.ID)
                {
                    References.AddExperience(enemy_Entity.ExpBonus);
                    References.AddCoin(enemy_Entity.CoinBonus);

                    MissionManager.Instance.DoingMission(AreaEnemy_Entity.EnemyID);

                }
            }

            gameObject.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                object[] data = new object[] { photonView.ViewID };
                PhotonNetwork.RaiseEvent((byte)CustomEventCode.EnemyDeactivate, data, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
            }

            Disappear();
        }

        LoadHealthUI(CurrentHealth, enemy_Entity.Health);

    }

    public void Disappear()
    {
        switch (BossType)
        {
            case BossType.BossType_Normal:
                CurrentHealth = enemy_Entity.Health;
                AreaEnemy_DAO.SetAreaEnemyDie(AreaID, EnemyID);
                Game_Manager.Instance.SpawnEnemyAfterDie(AreaID, EnemyID, photonView.ViewID, SpawnEnemyCoroutine);
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

            stream.SendNext(CurrentHealth);
            stream.SendNext(boss_Health);
            stream.SendNext(enemy_Entity.Health);
            stream.SendNext(transform.position);

            stream.SendNext(playerInRange);
            stream.SendNext(isMoving);

            stream.SendNext(TargetPosition);

            stream.SendNext(HealthChakraUI.GetComponent<RectTransform>().localScale);

        }
        else
        {

            CurrentHealth = (int)stream.ReceiveNext();
            boss_Health = (int)stream.ReceiveNext();
            enemy_Entity.Health = (int)stream.ReceiveNext();
            MovePosition = (Vector3)stream.ReceiveNext();

            playerInRange = (bool)stream.ReceiveNext();
            isMoving = (bool)stream.ReceiveNext();

            TargetPosition = (Vector3)stream.ReceiveNext();

            HealthChakraUI.GetComponent<RectTransform>().localScale = (Vector3)stream.ReceiveNext();
            if (BossType == BossType.BossType_Normal)
            {
                LoadHealthUI(CurrentHealth, enemy_Entity.Health);
            }
            else
            {
                LoadHealthUI(CurrentHealth, boss_Health);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(MainPoint.position, detectionRadius);

    }
}
