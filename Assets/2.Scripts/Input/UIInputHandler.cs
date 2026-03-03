using System;
using UnityEngine.InputSystem;

public sealed class UIInputHandler : InputActions.IUIActions
{
    Action _onExitAction;
    public Action OnExitAction { get { return _onExitAction; } }

    public event Action OnConfirmAction;

    public void OnExit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _onExitAction?.Invoke();
        }
    }
    public void OnConfirm(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OnConfirmAction?.Invoke();
        }
    }

}
