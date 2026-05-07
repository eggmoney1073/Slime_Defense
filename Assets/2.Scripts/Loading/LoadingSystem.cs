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

    // 로딩씬은 계속 유지
    static AsyncOperationHandle<SceneInstance> _loadingSceneHandle;

    // 로비/게임씬만 교체
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

        _isInitializing = true;

        AsyncOperationHandle handle = Addressables.InitializeAsync();

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

    /// <summary>
    /// 타이틀에서 최초 1회 호출.
    /// LoadingScene은 Single로 로드해서 타이틀을 밀어내고,
    /// 이후 Lobby/Game은 Additive로 교체한다.
    /// </summary>
    public static void LoadAddressableLoadingScene()
    {
        if (!_isAdreessableInitializeComplete)
        {
            Debug.LogError("Addressables not initialized yet.");
            return;
        }

        string sceneAddress = _sceneBaseAddress + SceneName.Scene_Loading + ".unity";

        _loadingSceneHandle = Addressables.LoadSceneAsync(sceneAddress, LoadSceneMode.Single);

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

    /// <summary>
    /// 로비/게임씬 전환용.
    /// LoadingScene은 건드리지 않는다.
    /// </summary>
    public static void LoadAddressableScene(SceneName sceneName)
    {
        if (!_isAdreessableInitializeComplete)
        {
            Debug.LogError("Addressables not initialized yet.");
            return;
        }

        if (_isChangingScene)
        {
            Debug.LogWarning("Scene is already changing.");
            return;
        }

        if (!IsContentScene(sceneName))
        {
            Debug.LogError($"Invalid content scene name: {sceneName}");
            return;
        }

        if (LoadingSceneManager.Instance == null)
        {
            Debug.LogError("LoadingSceneManager.Instance is null.");
            return;
        }

        LoadingSceneManager.Instance.StartCoroutine(ChangeContentScene(sceneName));
    }

    static IEnumerator ChangeContentScene(SceneName nextScene)
    {
        _isChangingScene = true;

        LoadingSceneManager.Instance.ShowUI();

        // 현재 게임씬이면 ECS SubScene 먼저 언로드
        if (_currentContentScene == SceneName.Scene_Game)
        {
            if (SubSceneLoader.Instance != null)
            {
                SubSceneLoader.Instance.UnloadSubScene();
            }
        }

        // 기존 로비/게임 Addressables 씬 언로드
        if (_contentSceneHandle.IsValid())
        {
            AsyncOperationHandle<SceneInstance> unloadHandle = Addressables.UnloadSceneAsync(_contentSceneHandle, true);

            yield return unloadHandle;

            _contentSceneHandle = default;
            _currentContentScene = SceneName.None;
        }

        // 3. BGM 변경
        PlaySceneBGM(nextScene);

        // 4. 새 로비/게임씬 Additive 로드
        string sceneAddress = _sceneBaseAddress + nextScene + ".unity";

        _contentSceneHandle = Addressables.LoadSceneAsync(sceneAddress, LoadSceneMode.Additive);

        yield return _contentSceneHandle;

        if (_contentSceneHandle.Status == AsyncOperationStatus.Succeeded)
        {
            _currentContentScene = nextScene;

            Debug.Log($"Content scene loaded successfully: {nextScene}");

            _onSceneLoadCompleted?.Invoke();
        }
        else
        {
            Debug.LogError($"Failed to load content scene: {nextScene}");
        }

        LoadingSceneManager.Instance.HideUI();

        _isChangingScene = false;
    }

    static bool IsContentScene(SceneName sceneName)
    {
        return sceneName == SceneName.Scene_Lobby ||
               sceneName == SceneName.Scene_Game;
    }

    static void PlaySceneBGM(SceneName sceneName)
    {
        if (SoundManager.Instance == null)
            return;

        if (sceneName == SceneName.Scene_Lobby)
        {
            SoundManager.Instance.PlayBGM(SoundManager.BGMType.MainMenu);
        }
        else if (sceneName == SceneName.Scene_Game)
        {
            SoundManager.Instance.PlayBGM(SoundManager.BGMType.Gameplay);
        }
    }
}