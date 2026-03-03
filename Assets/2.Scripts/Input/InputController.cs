using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : SingletonDontDestroyOnLoad<InputController>
{
    public enum InputStates
    {
        UI,
        InGame
    }

    bool _isInitialized = false;

    InputActions _inputController;

    InGameInputHandler _inGameInput;
    public InGameInputHandler InGameInput { get { return _inGameInput; } }

    UIInputHandler _uiInput;
    public UIInputHandler UIInput { get { return _uiInput; } }

    public void ChangeInputState(InputStates state)
    {
        _inputController.UI.Disable();
        _inputController.InGame.Disable();

        switch (state)
        {
            case InputStates.UI:
                _inputController.UI.Enable();
                break;
            case InputStates.InGame:
                _inputController.InGame.Enable();
                break;
        }
    }

    public void Initialize()
    {
        if (_isInitialized)
            return;

        _isInitialized = true;

        _inGameInput = new InGameInputHandler();
        _uiInput = new UIInputHandler();

        if (_inputController == null)
        {
            _inputController = new InputActions();
            _inputController.InGame.SetCallbacks(_inGameInput);
            _inputController.UI.SetCallbacks(_uiInput);
        }

        // 초기상태는 UI 모드로 설정
        ChangeInputState(InputStates.UI);
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        Initialize();
    }
}
