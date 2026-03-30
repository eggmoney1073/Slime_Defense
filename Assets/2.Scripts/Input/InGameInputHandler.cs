using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class InGameInputHandler : InputActions.IInGameActions
{
    Vector2 _moveInput;
    public Vector2 MoveInput { get { return _moveInput; } }

    public event Action OnPauseAction;

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnPauseAction?.Invoke();
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        float x = context.ReadValue<Vector2>().x;
        float z = context.ReadValue<Vector2>().y;

        Debug.Log($"Aim input: x={x}, z={z}");

        //AimDirectionBridge.SetAimDirection(x, z);
    }
}
