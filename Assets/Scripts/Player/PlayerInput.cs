using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float moveSmoothTime = 0.2f;

    private InputAction moveAction;

    private bool bWantsToMove;
    private Vector2 moveSmoothVelocity;
    private Vector2 moveInputVector;
    private Vector3 moveVector;
    public Vector3 MoveVector => moveVector;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.performed += MoveStart;
        moveAction.canceled += MoveStop;
    }

    private void MoveStart(InputAction.CallbackContext context)
    {
        moveInputVector = context.ReadValue<Vector2>();
    }

    private void MoveStop(InputAction.CallbackContext context)
    {
        moveInputVector = Vector2.zero;
    }

    private void LateUpdate()
    {
        moveVector = Vector2.SmoothDamp(moveVector, moveInputVector, ref moveSmoothVelocity, moveSmoothTime);
    }
}