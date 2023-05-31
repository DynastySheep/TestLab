using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonLib.PlayerInput;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 10f;

    private Rigidbody rb;
    private InputManager inputManager;

    float horizontalInput;
    float verticalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        InputManager inputManager = InputManager.GetInput();
        horizontalInput = inputManager.HorizontalInput;
        verticalInput = inputManager.VerticalInput;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        rb.MovePosition(rb.position + movement * walkSpeed * Time.fixedDeltaTime);
    }
}