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
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isCrouching = false;
    public PlayerState currentState;

    [Header("Player Control Variables")]
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private float jumpForce = 10f;


    [Header("Debugger Options")]
    [SerializeField] private bool debugMode = false;
    [SerializeField] private float jumpDetection_ray = 1f;

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

        CreateRaycast(transform.position, -Vector3.up, jumpDetection_ray);
        Debugging();
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.Frozen)
            return;

        PlayerMovement();
        PlayerJump();
    }

    private void PlayerMovement()
    {
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
    }

    private void PlayerJump()
    {
        if (isJumping && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isJumping = false;
            isGrounded = false;
        }
    }

    private void CreateRaycast(Vector3 position, Vector3 direction, float distance)
    {
        RaycastHit hit;
        Ray ray = new Ray(position, direction);

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, distance) && !isGrounded)
        {
            isGrounded = true;
        }
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
                    if (inputManager.GetAction(action) && !isJumping && isGrounded)
                    {
                        isJumping = true;
                    }
                    break;
            }
        }

        currentState = isFrozen ? PlayerState.Frozen : (isSprinting ? PlayerState.Sprinting : (isMoving ? PlayerState.Moving : PlayerState.Idle));
    }

    private void Debugging()
    {
        if (debugMode)
        {
            Debug.DrawRay(transform.position, -Vector3.up, Color.red);
        }
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