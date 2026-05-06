using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LobbyOptionController _optionController;

    public void Button_Start()
    {
        MainMenuManager.Instance.GoToNextScene();
    }

    public void Button_Options()
    {
        _optionController.ShowUI();
    }

    public void Button_Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
