using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LoadingSystem
{
    public enum SceneName
    {
        None = -1,
        Scene_Title,
        Scene_DownLoad,
        Scene_Loading,
        Scene_Lobby,
        Scene_Game,
        Max
    }

    static bool _isAdreessableInitializeComplete = false;
    static bool _isInitializing = false;
    static bool _isChangingScene = false;

    static string _sceneBaseAddress = "Assets/1.Scenes/";

    static AsyncOperationHandle<SceneInstance> _loadingSceneHandle;
    static AsyncOperationHandle<SceneInstance> _contentSceneHandle;

    static SceneName _currentContentScene = SceneName.None;

    public static Action _onSceneLoadCompleted;

    static float _loadingProcess = 0f;

    public static float LoadingProcess
    {
        get
        {
            return _loadingProcess;
        }
    }

    public static void InitializeAddressables(Action onCompleted = null)
    {
        if (_isInitializing)
            return;

        AsyncOperationHandle handle = Addressables.InitializeAsync();
        _isInitializing = true;

        handle.Completed += result =>
        {
            if (result.Status == AsyncOperationStatus.Succeeded)
            {
                _isAdreessableInitializeComplete = true;
                Debug.Log("Addressables initialized successfully.");
                onCompleted?.Invoke();
            }
            else
            {
                _isAdreessableInitializeComplete = false;
                Debug.LogError("Failed to initialize Addressables.");
            }

            _isInitializing = false;
        };
    }

    public static void LoadAddressableScene(SceneName sceneName)
    {
        if (_isChangingScene)
        {
            Debug.LogWarning("Scene is already changing.");
            return;
        }

        if (sceneName == SceneName.None || sceneName == SceneName.Max
            || sceneName == SceneName.Scene_Title || sceneName == SceneName.Scene_Loading
            || sceneName == SceneName.Scene_DownLoad)
        {
            Debug.LogError("Invalid scene name.");
            return;
        }

        if (!_isAdreessableInitializeComplete)
        {
            Debug.LogError("Addressables not initialized yet.");
            return;
        }

        if (LoadingSceneManager.Instance == null)
        {
            Debug.LogError("LoadingSceneManager.Instance is null.");
            return;
        }

        LoadingSceneManager.Instance.StartCoroutine(ChangeContentScene(sceneName));
    }

    static IEnumerator ChangeContentScene(SceneName sceneName)
    {
        _isChangingScene = true;
        _loadingProcess = 0f;

        LoadingSceneManager.Instance.ShowUI();

        bool wasGameScene = _currentContentScene == SceneName.Scene_Game;

        // 1. 기존 GameScene / LobbyScene 먼저 언로드
        if (_contentSceneHandle.IsValid())
        {
            AsyncOperationHandle<SceneInstance> unloadHandle = Addressables.UnloadSceneAsync(_contentSceneHandle, true);

            while (!unloadHandle.IsDone)
            {
                _loadingProcess = Mathf.Lerp(0f, 0.2f, unloadHandle.PercentComplete);
                yield return null;
            }

            _contentSceneHandle = default;
            _currentContentScene = SceneName.None;
            _loadingProcess = 0.2f;
        }
        else
        {
            _loadingProcess = 0.2f;
        }

        // 2. GameScene이 사라진 다음 ECS World 리셋
        if (wasGameScene)
        {
            yield return null;

            _loadingProcess = 0.3f;

            yield return EcsWorldResetter.ResetDefaultWorldRoutine();

            yield return null;

            _loadingProcess = 0.4f;
        }
        else
        {
            _loadingProcess = 0.4f;
        }

        // 3. BGM 변경
        if (sceneName == SceneName.Scene_Lobby)
        {
            SoundManager.Instance.PlayBGM(SoundManager.BGMType.MainMenu);
        }
        else if (sceneName == SceneName.Scene_Game)
        {
            SoundManager.Instance.PlayBGM(SoundManager.BGMType.Gameplay);
        }

        // 4. 새 씬 로드
        string sceneAddress = _sceneBaseAddress + sceneName.ToString() + ".unity";

        _contentSceneHandle = Addressables.LoadSceneAsync(
            sceneAddress,
            LoadSceneMode.Additive
        );

        while (!_contentSceneHandle.IsDone)
        {
            _loadingProcess = Mathf.Lerp(0.4f, 1f, _contentSceneHandle.PercentComplete);
            yield return null;
        }

        _loadingProcess = 1f;

        OnSceneLoadCompleted(_contentSceneHandle);

        if (_contentSceneHandle.Status == AsyncOperationStatus.Succeeded)
        {
            _currentContentScene = sceneName;
        }

        _isChangingScene = false;
    }

    public static void LoadAddressableLoadingScene()
    {
        if (!_isAdreessableInitializeComplete)
        {
            Debug.LogError("Addressables not initialized yet.");
            return;
        }

        string sceneAddress = _sceneBaseAddress + SceneName.Scene_Loading.ToString() + ".unity";

        _loadingSceneHandle = Addressables.LoadSceneAsync(
            sceneAddress,
            LoadSceneMode.Single
        );

        _loadingSceneHandle.Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Loading scene loaded successfully.");
                LoadAddressableScene(SceneName.Scene_Lobby);
            }
            else
            {
                Debug.LogError("Failed to load loading scene.");
            }
        };
    }

    private static void OnSceneLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Scene loaded successfully.");
            _onSceneLoadCompleted?.Invoke();
        }
        else
        {
            Debug.LogError("Failed to load scene.");
        }
    }
}