using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.dhcc.spaceshooter
{
    public class InputComp : MonoBehaviour
    {
        private InputAction moveAction;
        private InputAction sprintAction;

        public Vector2 MoveInput { get; private set; }

        public event Action<bool> OnSprintInput;
        public bool WantsToSprint { get; private set; }

        void Start()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            moveAction.performed += OnMoveAction;
            moveAction.canceled += OnMoveAction;

            sprintAction = InputSystem.actions.FindAction("Sprint");
            sprintAction.performed += OnSprintAction;
            sprintAction.canceled += OnSprintAction;
        }

        private void OnMoveAction(InputAction.CallbackContext context)
        {
            //TODO Is the switch statement even needed?
            //MoveInput = context.ReadValue<Vector2>();

            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    {
                        MoveInput = context.ReadValue<Vector2>();
                        return;
                    }
                case InputActionPhase.Canceled:
                    {
                        MoveInput = Vector2.zero;
                        return;
                    }
            }
        }

        private void OnSprintAction(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    {
                        WantsToSprint = true;
                        OnSprintInput?.Invoke(WantsToSprint);
                        return;
                    }
                case InputActionPhase.Canceled:
                    {
                        WantsToSprint = false;
                        OnSprintInput?.Invoke(WantsToSprint);
                        return;
                    }
            }
        }
    }
}