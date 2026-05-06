using UnityEngine;
using UnityEngine.UI;

public class LobbyOptionController : MonoBehaviour, IWindow
{
    [Header("References")]
    [SerializeField] private Slider _volumeSliderBGM;
    [SerializeField] private Slider _volumeSliderSFX;
    [SerializeField] private Button _saveButton;


    public void Initialize()
    {
        _saveButton.onClick.AddListener(OnSaveButtonClicked);

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

    private void OnSaveButtonClicked()
    {
        HideUI();
    }

    private void SetVolumeSettings()
    {
        SoundManager.Instance.BGMVolume = _volumeSliderBGM.value;
        SoundManager.Instance.SFXVolume = _volumeSliderSFX.value;
    }
}
