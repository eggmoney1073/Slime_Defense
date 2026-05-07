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

    public static float LoadingProcess
    {
        get
        {
            if (_contentSceneHandle.IsValid())
                return _contentSceneHandle.PercentComplete;

            if (_loadingSceneHandle.IsValid())
                return _loadingSceneHandle.PercentComplete;

            return 0f;
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

        if (sceneName == SceneName.Scene_Lobby)
        {
            SoundManager.Instance.PlayBGM(SoundManager.BGMType.MainMenu);
        }
        else if (sceneName == SceneName.Scene_Game)
        {
            SoundManager.Instance.PlayBGM(SoundManager.BGMType.Gameplay);
        }

        // 기존 코드처럼 로드 전에 ShowUI만 호출
        LoadingSceneManager.Instance.ShowUI();

        // 게임씬에서 나갈 때 ECS SubScene 먼저 언로드
        if (_currentContentScene == SceneName.Scene_Game)
        {
            yield return EcsWorldResetter.ResetDefaultWorldRoutine();
        }

        // 기존 컨텐츠 씬 언로드 완료까지 대기
        if (_contentSceneHandle.IsValid())
        {
            yield return Addressables.UnloadSceneAsync(_contentSceneHandle, true);
            _contentSceneHandle = default;
            _currentContentScene = SceneName.None;
        }

        string sceneAddress = _sceneBaseAddress + sceneName.ToString() + ".unity";

        _contentSceneHandle = Addressables.LoadSceneAsync(
            sceneAddress,
            LoadSceneMode.Additive
        );

        yield return _contentSceneHandle;

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