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
        Scene_Loading,
        Scene_MainMenu,
        Scene_GameLobby,
        Scene_GameField,
        Max
    }

    static bool _isAdreessableInitializeComplete = false;
    static bool _isInitializing= false;
    static string _sceneBaseAddress = "Assets/1.Scenes/";
    static AsyncOperationHandle<SceneInstance> _sceneLoadHandle;
    public static Action _onSceneLoadCompleted;

    /// <summary>
    /// 로딩 진행률
    /// </summary>
    public static float LoadingProcess 
    { get 
        {
            if (_sceneLoadHandle.IsValid())
                return _sceneLoadHandle.PercentComplete;
            else
                return 0f;
        }
    }

    /// <summary>
    /// Addressables 초기화
    /// </summary>
    public static void InitializeAddressables(Action onCompleted = null)
    {
        // 이미 초기화 중인지 확인
        if (_isInitializing)
            return;

        AsyncOperationHandle  hndle = Addressables.InitializeAsync();
        _isInitializing = true;

        hndle.Completed += result =>
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
    /// 씬 로드 시도
    /// </summary>
    /// <param name="sceneName"></param>
    public static void LoadAddressableScene(SceneName sceneName)
    {
        // 씬 이름 유효성 검사
        // Title 씬은 로드하지 않음
        // Loading 씬은 단독으로 로드
        if (sceneName == SceneName.None || sceneName == SceneName.Max 
            || sceneName == SceneName.Scene_Title || sceneName == SceneName.Scene_Loading)
        {
            Debug.LogError("Invalid scene name.");
            return;
        }

        // 이미 로딩 중인지 확인
        if (_sceneLoadHandle.IsValid() && !_sceneLoadHandle.IsDone)
        {
            Debug.LogWarning("Scene is already loading.");
            return;
        }

        // Addressables 초기화 여부 확인
        if (!_isAdreessableInitializeComplete)
        {
            Debug.LogError("Addressables not initialized yet.");
            return;
        }

        string sceneAddress = _sceneBaseAddress + sceneName.ToString() + ".unity";

        LoadingSceneManager.Instance.ShowUI(_onSceneLoadCompleted);
        _sceneLoadHandle = Addressables.LoadSceneAsync(sceneAddress, LoadSceneMode.Additive);
        

        // 콜백함수 등록
        _sceneLoadHandle.Completed += OnSceneLoadCompleted;
    }


    /// <summary>
    /// 로딩 씬은 타이틀에서 한번만 로드
    /// </summary>
    public static void LoadAddressableLoadingScene()
    {
        string sceneAddress = _sceneBaseAddress + SceneName.Scene_Loading.ToString() + ".unity";
        _sceneLoadHandle = Addressables.LoadSceneAsync(sceneAddress, LoadSceneMode.Single);

        _sceneLoadHandle.Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Loading scene loaded successfully.");
                LoadAddressableScene(SceneName.Scene_MainMenu);
            }
            else
            {
                Debug.LogError("Failed to load loading scene.");
            }
        };        
    }


    /// <summary>
    /// 씬 로드 완료 콜백
    /// </summary>
    /// <param name="handle"></param>
    private static void OnSceneLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // 로드 성공 시
            Debug.Log("Scene loaded successfully.");
            _onSceneLoadCompleted?.Invoke();
        }
        else
        {
            // 로드 실패 시
            Debug.LogError("Failed to load scene.");
        }
    }


    /// <summary>
    /// 이전 씬 언로드
    /// </summary>
    public static void UnloadCurrentScene()
    {
        if (_sceneLoadHandle.IsValid())
        {
            Addressables.UnloadSceneAsync(_sceneLoadHandle);
        }
    }
}
