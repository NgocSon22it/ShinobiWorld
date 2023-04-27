using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerBase : MonoBehaviour
{

    //Component
    public Animator animator;

    //Player Input
    PlayerInput playerInput;
    private InputAction Input_AttackAction;
    private InputAction Input_MoveAction;
    private InputAction Input_Skill1;
    [SerializeField] float Speed;
    [SerializeField] Vector2 MoveDirection;
    Vector3 Movement;


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
    }

    public void Start()
    {
        SetUpInput();
        SetUpComponent();
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
        Walk();
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



}
