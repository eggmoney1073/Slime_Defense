using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour, IWindow
{
    [Header("References")]
    [SerializeField] private Button _exitButton;

    private void Start()
    {
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }
    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    private void OnExitButtonClicked()
    {
        LoadingSystem.LoadAddressableScene(LoadingSystem.SceneName.Scene_Lobby);
        HideUI();
    }
}
