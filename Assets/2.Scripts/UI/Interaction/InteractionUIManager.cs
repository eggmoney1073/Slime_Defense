using UnityEngine;

public class InteractionUIManager : SingletonGameobject<InteractionUIManager>
{
    [Header("Interaction UI Controllers")]
    [SerializeField] private PauseWindowController _pauseController;
    [SerializeField] private LevelUpController _levelUpController;
    [SerializeField] private JoystickInput _joystickInput;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _pauseController.Initialize();

        GameFlowManager.Instance.SubscribeGameState(GameFlowManager.GameState.Pause, () =>
        {
            _joystickInput.IsPaused = true;
            _pauseController.ShowUI();
        });
        GameFlowManager.Instance.SubscribeGameState(GameFlowManager.GameState.Playing, () =>
        {
            _joystickInput.IsPaused = false;
            _pauseController.HideUI();
        });
        GameFlowManager.Instance.SubscribeGameState(GameFlowManager.GameState.LevelUp, () =>
        {
            _joystickInput.IsPaused = true;
        });
    }
}
