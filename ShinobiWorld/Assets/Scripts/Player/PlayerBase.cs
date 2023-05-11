using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Rendering;

using System;
using TMPro;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerBase : MonoBehaviour, IPunObservable
{

    [Header("Player Entity")]
    public Account_Entity PlayerEntity;
    int CurrentHealth, CurrentChakra;


    [Header("Player Instance")]
    [SerializeField] GameObject PlayerControlPrefabs;
    [SerializeField] GameObject PlayerCameraPrefabs;
    [SerializeField] TMP_Text PlayerNickName;
    [SerializeField] GameObject PlayerCommonUI;
    GameObject PlayerControlInstance;
    GameObject PlayerCameraInstance;

    //Attack
    [SerializeField] public Transform AttackPoint;

    //Skill
    public float SkillOneCooldown_Total;
    public float SkillOneCooldown_Current;

    public float SkillTwoCooldown_Total;
    public float SkillTwoCooldown_Current;

    public float SkillThreeCooldown_Total;
    public float SkillThreeCooldown_Current;

    //Take Damage
    private bool Hurting;

    //Enemy
    protected GameObject Enemy;

    //Component
    public Animator animator;
    public PhotonView PV;
    public Rigidbody2D rigidbody2d;
    public SpriteRenderer spriteRenderer;
    public SortingGroup sortingGroup;
    public PlayerInput playerInput;

    //Script Component
    public PlayerPool playerPool;

    //Player Input
    [SerializeField] float Speed;
    [SerializeField] Vector2 MoveDirection;
    Vector3 Movement;
    bool FacingRight = true;


    Vector3 realPosition;
    Quaternion realRotation;
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    Vector3 positionAtLastPacket = Vector3.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;


    public void SetUpComponent()
    {
        animator = GetComponent<Animator>();
        PV = GetComponent<PhotonView>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sortingGroup = GetComponent<SortingGroup>();
        playerInput = GetComponent<PlayerInput>();
        playerPool = GetComponent<PlayerPool>();
    }

    public void Start()
    {
        SetUpComponent();

        PlayerNickName.text = PV.Owner.NickName;

        if (PV.IsMine)
        {
            PlayerControlInstance = Instantiate(PlayerControlPrefabs);
            PlayerCameraInstance = Instantiate(PlayerCameraPrefabs);
            PlayerCameraInstance.GetComponent<CinemachineVirtualCamera>().m_Follow = gameObject.transform;
            PlayerControlInstance.GetComponent<PlayerUI>().SetUpPlayer(this.gameObject);
            sortingGroup.sortingLayerName = "Me";
            PlayerCommonUI.GetComponent<Canvas>().sortingLayerName = "Me";
        }
        else
        {
            sortingGroup.sortingLayerName = "Other";
            PlayerCommonUI.GetComponent<Canvas>().sortingLayerName = "Other";
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
    }

    public void Update()
    {
        animator.SetFloat("Horizontal", MoveDirection.x);
        animator.SetFloat("Vertical", MoveDirection.y);
        animator.SetFloat("Speed", MoveDirection.sqrMagnitude);
    }

    public void FixedUpdate()
    {
        if (PV.IsMine)
        {
            Walk();
        }
        else
        {
            rigidbody2d.isKinematic = true;
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            //Update remote player
            transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, (float)(currentTime / timeToReachGoal));

        }
    }

    public void TakeDamage(int Damage)
    {
        if (Hurting) { return; }
        CurrentHealth -= Damage;
        StartCoroutine(DamageAnimation());
        PlayerCameraInstance.GetComponent<PlayerCamera>().StartShakeScreen(3, 3, 1);

        if (CurrentHealth <= 0)
        {
            Debug.Log("Die");
        }
    }

    public IEnumerator DamageAnimation()
    {
        Hurting = true;
        for (int i = 0; i < 10; i++)
        {
            spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(.1f);

            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(.1f);
        }
        Hurting = false;
    }

    [PunRPC]
    public void FindClostestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        GameObject closestEnemy = null;
        GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy");


        foreach (GameObject currentEnemy in allEnemy)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }
        if (closestEnemy != null)
        {
            Enemy = closestEnemy;
        }
    }

    public void Walk()
    {
        Movement = new Vector3(MoveDirection.x, MoveDirection.y, 0f);
        transform.Translate(Movement * Speed * Time.fixedDeltaTime);

        if (Movement.x > 0 && !FacingRight)
        {
            Flip();
        }
        else if (Movement.x < 0 && FacingRight)
        {
            Flip();
        }
    }
    public void Flip()
    {
        FacingRight = !FacingRight;
        if (FacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            PlayerCommonUI.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            PlayerCommonUI.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
        }
    }
    public void FlipToEnemy()
    {
        if (Enemy != null)
        {
            if (Enemy.transform.position.x > AttackPoint.position.x && !FacingRight)
            {
                Flip();
            }
            else if (Enemy.transform.position.x < AttackPoint.position.x && FacingRight)
            {
                Flip();
            }
        }
    }

    [PunRPC]
    public void TriggerAnimator(string TriggerName)
    {
        animator.SetTrigger(TriggerName);
    }

    public void CallSyncAnimation(string TriggerName)
    {
        PV.RPC(nameof(TriggerAnimator), RpcTarget.AllBuffered, TriggerName);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(MoveDirection);
            stream.SendNext(PlayerCommonUI.GetComponent<RectTransform>().localScale);
        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            MoveDirection = (Vector2)stream.ReceiveNext();
            PlayerCommonUI.GetComponent<RectTransform>().localScale = (Vector3)stream.ReceiveNext();

            //Lag compensation
            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;
        }
    }

}
