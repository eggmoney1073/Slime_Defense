using UnityEngine;

public class MainMenuManager : SingletonGameobject<MainMenuManager>
{
    [SerializeField]
    Camera _mainCamera;

    void OnLoadMainMenuSceneCompleted()
    {
        Debug.Log("MainMenu Scene Load Completed");
        _mainCamera.enabled = true;
    }

    void Start()
    {
        _mainCamera.enabled = false;
        LoadingSystem._onSceneLoadCompleted += OnLoadMainMenuSceneCompleted;
    }

    void OnDestroy()
    {
        LoadingSystem._onSceneLoadCompleted -= OnLoadMainMenuSceneCompleted;
    }

    public void GoToNextScene()
    {
        LoadingSystem._onSceneLoadCompleted -= OnLoadMainMenuSceneCompleted;
        LoadingSystem.UnloadCurrentScene();
        LoadingSystem.LoadAddressableScene(LoadingSystem.SceneName.Scene_GameLobby);
    }
}
