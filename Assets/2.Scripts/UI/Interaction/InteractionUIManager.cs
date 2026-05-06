using UnityEngine;

public class InteractionUIManager : SingletonGameobject<InteractionUIManager>
{
    [Header("Interaction UI Controllers")]
    [SerializeField] private PauseWindowController _pauseController;
    [SerializeField] private LevelUpController _levelUpController;
    [SerializeField] private JoystickInput _joystickInput;
    [SerializeField] private GameOverWindow _gameOverWindow;
    [SerializeField] private GameClearWindow _gameClearWindow;

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
        GameFlowManager.Instance.SubscribeGameState(GameFlowManager.GameState.GameOver, () =>
        {
            _joystickInput.IsPaused = true;
            _gameOverWindow.ShowUI();
        });
        GameFlowManager.Instance.SubscribeGameState(GameFlowManager.GameState.Clear, () =>
        {
            _joystickInput.IsPaused = true;
            _gameClearWindow.ShowUI();
        });
    }

    public void GoToLobby()
    {
        LoadingSystem.UnloadCurrentScene();
        LoadingSystem.LoadAddressableScene(LoadingSystem.SceneName.Scene_Lobby);
    }
}
