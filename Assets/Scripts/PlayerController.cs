using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonLib.PlayerInput;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Player State Bools")]
    [SerializeField] private bool isFrozen = true; // Freeze at start for now
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isSprinting = false;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isCrouching = false;
    public PlayerState currentState;

    [Header("Player Control Variables")]
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private float jumpForce = 10f;


    private InputManager inputManager;
    private Rigidbody rb;

    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentState = PlayerState.Frozen;
    }

    private void Update()
    {
        InputHandler(); // Deal with the input
        StateHandler(); // First check state to make sure that player is not frozen
    }


    private void FixedUpdate()
    {
        if (currentState == PlayerState.Frozen)
            return;

        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
    }

    private void PlayerJump()
    {

    }

    private void StateHandler()
    {
        if (currentState == PlayerState.Frozen)
        return;

        if (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0)
            isMoving = true;
        else
            if (isMoving)
                isMoving = false;
    }

    private void InputHandler()
    {
        inputManager = InputManager.GetInput();
        horizontalInput = inputManager.HorizontalInput;
        verticalInput = inputManager.VerticalInput;

        foreach (InputAction action in Enum.GetValues(typeof(InputAction)))
        {
            switch(action)
            {
                case InputAction.Freeze:
                    if (inputManager.GetAction(action))
                    {
                        isFrozen = !isFrozen;
                    }
                    break;

                case InputAction.Sprint:
                    if (inputManager.GetAction(action))
                    {
                        isSprinting = currentState == PlayerState.Moving || currentState == PlayerState.Sprinting;
                    }
                    else
                    {
                        if (isSprinting)
                            isSprinting = false;
                    }
                    break;

                case InputAction.Jump:
                    if (inputManager.GetAction(action) && !isJumping)
                    {
                        isJumping = true;
                    }
                    break;
            }
        }

        currentState = isFrozen ? PlayerState.Frozen : (isSprinting ? PlayerState.Sprinting : (isMoving ? PlayerState.Moving : PlayerState.Idle));
    }
}

public enum PlayerState
{
    Frozen,
    Idle,
    Moving,
    Sprinting,
    Crouching
}