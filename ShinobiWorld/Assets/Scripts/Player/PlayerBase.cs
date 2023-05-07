using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

public class PlayerBase : MonoBehaviour, IPunObservable
{

    [SerializeField] GameObject PlayerControl;
    [SerializeField] GameObject PlayerCamera;

    GameObject Enemy;

    //Component
    public Animator animator;
    public PhotonView PV;
    public Rigidbody2D rigidbody2d;
    public SpriteRenderer spriteRenderer;
    public SortingGroup sortingGroup;

    //Player Input
    PlayerInput playerInput;
    [SerializeField] float Speed;
    [SerializeField] Vector2 MoveDirection;
    Vector3 Movement;


    Vector3 realPosition;
    Quaternion realRotation;
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    Vector3 positionAtLastPacket = Vector3.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;

    public void SetUpInput()
    {
        playerInput = GetComponent<PlayerInput>();

    }

    public void SetUpComponent()
    {
        animator = GetComponent<Animator>();
        PV = GetComponent<PhotonView>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sortingGroup = GetComponent<SortingGroup>();
    }

    public void Start()
    {
        SetUpInput();
        SetUpComponent();
        
        if (PV.IsMine)
        {
            Instantiate(PlayerControl);
            GameObject Camera = Instantiate(PlayerCamera);
            Camera.GetComponent<CinemachineVirtualCamera>().m_Follow = gameObject.transform;
            sortingGroup.sortingLayerName = "Me";
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
    }

    public void OnSkillOne(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PV.RPC(nameof(FindClostestEnemy), RpcTarget.AllBuffered);
            Debug.Log(Enemy.name);
        }
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

        if (Movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(MoveDirection);
        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            MoveDirection = (Vector2)stream.ReceiveNext();

            //Lag compensation
            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;

        }
    }
}
