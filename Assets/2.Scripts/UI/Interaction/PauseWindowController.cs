using UnityEngine;
using UnityEngine.UI;

public class PauseWindowController : MonoBehaviour, IWindow
{
    [Header("References")]
    [SerializeField] private Slider _volumeSliderBGM;
    [SerializeField] private Slider _volumeSliderSFX;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _lobbyButton;


    public void Initialize()
    {
        _resumeButton.onClick.AddListener(OnResumeButtonClicked);
        _lobbyButton.onClick.AddListener(OnLobbyButtonClicked);

        _volumeSliderBGM.value = SoundManager.Instance.BGMVolume;
        _volumeSliderSFX.value = SoundManager.Instance.SFXVolume;
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
        Initialize();
    }

    public void HideUI()
    {
        SetVolumeSettings();
        gameObject.SetActive(false);
    }

    private void OnResumeButtonClicked()
    {
        GameFlowManager.Instance.ChangeGameState(GameFlowManager.GameState.Playing);
        HideUI();
    }

    private void OnLobbyButtonClicked()
    {
        LoadingSystem.LoadAddressableScene(LoadingSystem.SceneName.Scene_Lobby);
        HideUI();
    }

    private void SetVolumeSettings()
    {
        SoundManager.Instance.BGMVolume = _volumeSliderBGM.value;
        SoundManager.Instance.SFXVolume = _volumeSliderSFX.value;
    }
}
