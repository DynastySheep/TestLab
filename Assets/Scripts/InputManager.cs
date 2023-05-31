using UnityEngine;
using System.Collections.Generic;

namespace MoonLib.PlayerInput
{
    public struct InputManager
    {
        public float HorizontalInput { get; private set; }
        public float VerticalInput { get; private set; }
        public float MouseX { get; private set; }
        public float MouseY { get; private set; }
        private Dictionary<InputAction, bool> inputActions;

        public InputManager(float horizontal, float vertical, float mouseX, float mouseY, Dictionary<InputAction, bool> actions)
        {
            HorizontalInput = horizontal;
            VerticalInput = vertical;
            MouseX = mouseX;
            MouseY = mouseY;
            inputActions = actions;
        }
        
        public bool GetAction(InputAction action)
        {
            if (inputActions.ContainsKey(action))
                return inputActions[action];
            else
                return false;
        }

        public static InputManager GetInput()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Dictionary<InputAction, bool> actions = new Dictionary<InputAction, bool>();
            actions[InputAction.Jump] = Input.GetButtonDown("Jump");
            actions[InputAction.Crouch] = Input.GetButtonDown("Crouch");
            actions[InputAction.Run] = Input.GetButton("Run");
            actions[InputAction.Shoot] = Input.GetButtonDown("Shoot");

            return new InputManager(horizontalInput, verticalInput, mouseX, mouseY, actions);
        }
    }

    public enum InputAction
    {
        Jump,
        Crouch,
        Run,
        Shoot
    }
}