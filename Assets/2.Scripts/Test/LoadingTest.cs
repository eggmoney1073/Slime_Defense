using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingTest : MonoBehaviour
{
    public void LoadScene()
    {
        LoadingSystem.UnloadCurrentScene();
        LoadingSystem.LoadAddressableScene(LoadingSystem.SceneName.Scene_GameLobby);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "Load Lobby Scene"))
        {
            MainMenuManager.Instance.GoToNextScene();
        }

        if (GUI.Button(new Rect(10, 70, 150, 50), "Initialize Addressables"))
        {
            LoadingSystem.InitializeAddressables();
        }
    }
}
