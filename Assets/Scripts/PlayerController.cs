using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonLib.PlayerInput;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private InputManager inputManager;
    public PlayerState currentState;

    private Rigidbody rb;

    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    private bool isFrozen = true; // Freeze at start for now
    private bool isMoving = false;
    private bool isRunning = false;
    private bool isJumping = false;
    private bool isCrouching = false;

    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float runSpeed = 15f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentState = PlayerState.Frozen;
    }

    private void Update()
    {
        StateHandler(); // First check state to make sure that player is not frozen
        InputHandler(); // Deal with the input

        if (inputManager.GetAction(InputAction.Freeze) == true)
            isFrozen = !isFrozen;

        currentState = isFrozen ? PlayerState.Frozen : (isMoving ? PlayerState.Moving : PlayerState.Idle);
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.Frozen)
            return;

        PlayerMovement();
    }

    private void PlayerMovement()
    {
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        rb.MovePosition(rb.position + movement * walkSpeed * Time.fixedDeltaTime);
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
    }
}

public enum PlayerState
{
    Frozen,
    Idle,
    Moving,
    Running,
    Crouching
}