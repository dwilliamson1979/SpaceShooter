using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private InputAction moveAction;

    public Vector2 MoveInput { get; private set; }

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.performed += OnMoveAction;
        moveAction.canceled += OnMoveAction;
    }

    private void OnMoveAction(InputAction.CallbackContext context)
    {
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
}