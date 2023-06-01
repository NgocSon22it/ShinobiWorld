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

public class Enemy : MonoBehaviourPunCallbacks, IPunObservable
{
    // Entity
    protected Boss_Entity boss_Entity = new Boss_Entity();
    int CurrentHealth;

    // Move Area
    [SerializeField] Collider2D movementBounds;
    Vector2 minPosition, maxPosition;
    float randomX, randomY;
    Vector2 randomPosition;

    private Vector3 targetPosition;
    private bool isMoving = true;
    private float delayTimer;
    protected GameObject Target;
    private bool playerInRange = false;
    Vector3 clampedPosition;
    public float detectionRadius = 5f;
    public float delayTime = 2f;

    private float timer = 0f;
    private float interval = 1f;


    // MainPoint
    [SerializeField] Transform MainPoint;

    //Health UI
    [SerializeField] Image CurrentHealth_UI;
    [SerializeField] TMP_Text HealthNumber;

    // Skill Direction
    public Vector2 direction;

    //Scale Value
    float ScaleValue;

    // Health Bar
    [SerializeField] GameObject HealthChakraUI;

    //Component
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigidbody2d;
    public Animator animator;
    public Boss_Pool boss_Pool;

    // Facing
    public bool FacingRight = true;

    public void SetUpComponent()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boss_Pool = GetComponent<Boss_Pool>();
    }

    public void Start()
    {
        SetUpComponent();
        targetPosition = GetRandomPosition();
        if (photonView.IsMine)
        {
            if (boss_Entity.ID != null)
            {
                boss_Entity = Boss_DAO.GetBossByID(boss_Entity.ID);
                CurrentHealth = boss_Entity.Health;
                ScaleValue = transform.localScale.y;
            }
        }

        LoadHealthUI();
    }

    public void Update()
    {

    }

    public void LoadHealthUI()
    {
        HealthNumber.text = CurrentHealth + " / " + boss_Entity.Health;
        CurrentHealth_UI.fillAmount = (float)CurrentHealth / (float)boss_Entity.Health;
    }

    public void TakeDamage(string UserID, int Damage)
    {
        photonView.RPC(nameof(TakeDamageSync), RpcTarget.AllBuffered, UserID, Damage);
    }

    public void AttackAndMoveRandom()
    {
        if (playerInRange)
        {
            // Stop moving
            isMoving = false;
        }
        else
        {
            if (isMoving)
            {
                // Move towards the target position
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, 3f * Time.deltaTime);

                // Check if the NPC has reached the target position
                if (transform.position == targetPosition)
                {
                    // Start the delay timer
                    delayTimer = delayTime;
                    isMoving = false;
                }

            }
            else
            {

                // Start or continue the delay
                delayTimer -= Time.deltaTime;
                if (delayTimer <= 0f)
                {
                    // Set a new random target position and start moving again
                    targetPosition = GetRandomPosition();
                    isMoving = true;
                }
            }
        }
        timer += Time.deltaTime;

        // Check if the interval has passed
        if (timer >= interval)
        {
            if (FindClostestTarget(detectionRadius, "Player") != null)
            {
                photonView.RPC(nameof(SyncFindTarget), RpcTarget.AllBuffered);
            }
            else
            {
                Target = null;
                Debug.Log("Reduce");
            }
            // Call the RPC and reset the timer
            timer = 0f;
        }

        // Update the player in range status
        playerInRange = Target != null;

        // Restrict movement to the move area
        clampedPosition = movementBounds.ClosestPoint(transform.position);
        transform.position = new Vector3(clampedPosition.x, clampedPosition.y, transform.position.z);

        animator.SetBool("Attack", playerInRange);
        animator.SetBool("Walk", isMoving);
    }


    [PunRPC]
    public void TakeDamageSync(string UserID, int Damage)
    {
        CurrentHealth -= Damage;
        LoadHealthUI();
        if (CurrentHealth < 0)
        {
            Debug.Log(UserID);
        }
    }

    private Vector2 GetRandomPosition()
    {
        // Get the bounds of the collider
        minPosition = movementBounds.bounds.min;
        maxPosition = movementBounds.bounds.max;

        // Generate random X and Y coordinates within the collider bounds
        randomX = Random.Range(minPosition.x, maxPosition.x);
        randomY = Random.Range(minPosition.y, maxPosition.y);

        // Create a new position using the random coordinates
        randomPosition = new Vector2(randomX, randomY);

        if (randomPosition.x > MainPoint.position.x && !FacingRight)
        {
            Flip();
        }
        else if (randomPosition.x < MainPoint.position.x && FacingRight)
        {
            Flip();
        }
        return randomPosition;
    }

    public void Flip()
    {
        FacingRight = !FacingRight;

        if (!FacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            HealthChakraUI.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            HealthChakraUI.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
        }
    }

    public void FlipToTarget()
    {
        if (photonView.IsMine)
        {
            if (Target.transform.position.x > MainPoint.position.x && !FacingRight)
            {
                Flip();
            }
            else if (Target.transform.position.x < MainPoint.position.x && FacingRight)
            {
                Flip();
            }
        }
    }

    [PunRPC]
    public void SyncFindTarget()
    {
        Target = FindClostestTarget(detectionRadius, "Player");
    }


    public GameObject FindClostestTarget(float Range, string TargetTag)
    {
        float distanceToClosestTarget = Mathf.Infinity;
        GameObject closestTarget = null;
        GameObject[] allTarget = GameObject.FindGameObjectsWithTag(TargetTag);


        foreach (GameObject currentTarget in allTarget)
        {
            float distanceToTarget = (currentTarget.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToTarget < distanceToClosestTarget && Vector2.Distance(currentTarget.transform.position, transform.position) <= Range)
            {
                distanceToClosestTarget = distanceToTarget;
                closestTarget = currentTarget;

            }
        }

        return closestTarget;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CurrentHealth);
            stream.SendNext(boss_Entity.Health);
            stream.SendNext(transform.position);

            stream.SendNext(playerInRange);
            stream.SendNext(isMoving);

            stream.SendNext(HealthChakraUI.GetComponent<RectTransform>().localScale);
        }
        else
        {
            CurrentHealth = (int)stream.ReceiveNext();
            boss_Entity.Health = (int)stream.ReceiveNext();
            targetPosition = (Vector3)stream.ReceiveNext();

            playerInRange = (bool)stream.ReceiveNext();
            isMoving = (bool)stream.ReceiveNext();

            HealthChakraUI.GetComponent<RectTransform>().localScale = (Vector3)stream.ReceiveNext();
        }
    }
}
