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
    protected int CurrentHealth;

    // Move Area
    [SerializeField] public Collider2D movementBounds;
    Vector2 minPosition, maxPosition;
    float randomX, randomY;
    Vector2 randomPosition;


    protected Vector3 MovePosition;
    public bool isMoving = true;

    public float Break_CurrentTime;
    public float Break_TotalTime = 2f;

    protected GameObject Target;
    protected bool playerInRange = false;
    public Vector3 clampedPosition;
    public float detectionRadius = 5f;

    public float FindTarget_CurrentTime;
    public float FindTarget_TotalTime = 1f;


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
    public bool FacingRight;

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
        MovePosition = GetRandomPosition();
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
        if (FacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            HealthChakraUI.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            HealthChakraUI.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
        }

        FacingRight = !FacingRight;
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
            MovePosition = (Vector3)stream.ReceiveNext();

            playerInRange = (bool)stream.ReceiveNext();
            isMoving = (bool)stream.ReceiveNext();

            HealthChakraUI.GetComponent<RectTransform>().localScale = (Vector3)stream.ReceiveNext();
        }
    }
}
