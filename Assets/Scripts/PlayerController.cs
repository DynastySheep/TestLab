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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        inputManager = InputManager.GetInput();
    }

    private void FixedUpdate()
    {
        float horizontalInput = inputManager.HorizontalInput;
        float verticalInput = inputManager.VerticalInput;

        Debug.Log(horizontalInput);

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        rb.MovePosition(rb.position + movement * walkSpeed * Time.fixedDeltaTime);
    }
}