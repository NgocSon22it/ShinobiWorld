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

    [SerializeField] GameObject ControlUI;
    [SerializeField] GameObject Camera;

    //Component
    public Animator animator;
    public PhotonView PV;
    public Rigidbody2D rigidbody2d;
    public SpriteRenderer spriteRenderer;
    public SortingGroup sortingGroup;

    //Player Input
    PlayerInput playerInput;
    private InputAction Input_AttackAction;
    private InputAction Input_MoveAction;
    private InputAction Input_Skill1;
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
        Input_AttackAction = playerInput.actions["Attack"];
        Input_MoveAction = playerInput.actions["Move"];
        //Input_Skill1 = playerInput.actions["Skill 1"];
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
            Instantiate(ControlUI);
            GameObject PlayerCamera = Instantiate(Camera);
            PlayerCamera.GetComponent<CinemachineVirtualCamera>().m_Follow = gameObject.transform;
            sortingGroup.sortingLayerName = "Me";
        }
    }

    public void Update()
    {
        if (Input_AttackAction.triggered)
        {
            Debug.Log("Attack");
        }
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

    public void Walk()
    {
        MoveDirection = Input_MoveAction.ReadValue<Vector2>();
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
