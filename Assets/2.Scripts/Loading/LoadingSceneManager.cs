using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
//using UnityEngine.UIElements;

/// <summary>
/// 로딩 씬을 관리하는 매니저
/// </summary>
/// 로딩씬은 언로드 하지 않고 계속 유지하므로 Update를 사용하지 않는다.
public class LoadingSceneManager : SingletonGameobject<LoadingSceneManager>
{
    [SerializeField]
    Image _loadingBGImage;
    [SerializeField]
    Slider _loadingBar;
    [SerializeField]
    GameObject _titleImage;
    [SerializeField]
    GameObject _touchToStartText;
    [SerializeField]
    GameObject _touchToStartButton;
    [SerializeField]
    GameObject _eventSystem;



    CanvasGroup _canvasGroup;
    Action _fadeOutCallBack;


    #region [ Loading Fuction ]    

    /// <summary>
    /// UI 보이기
    /// </summary>
    public void ShowUI(Action fadeOutCallBack = null)
    {
        SetRandomBG();
        _fadeOutCallBack = fadeOutCallBack;
        StartCoroutine(Co_FadeIn(0.5f));
        _canvasGroup.blocksRaycasts = true;
        //_eventSystem.SetActive(true);
    }

    void SetRandomBG()
    {
        int randomIndex = UnityEngine.Random.Range(0, LoadingResourceManager._loadingBGCount);
        _loadingBGImage.sprite = LoadingResourceManager.GetLoadingBG(randomIndex);
    }

    public void HideUI()
    {
        StartCoroutine(Co_FadeOut(1f));
        _canvasGroup.blocksRaycasts = false;
        _fadeOutCallBack?.Invoke();
    }

    IEnumerator Co_FadeIn(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
        _canvasGroup.alpha = 1f;

        if (_titleImage != null)
            Destroy(_titleImage);

        StartCoroutine(Co_SetLoadingBar());
    }

    IEnumerator Co_FadeOut(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
        _canvasGroup.alpha = 0f;
    }

    IEnumerator Co_SetLoadingBar()
    {
        while (LoadingSystem.LoadingProcess > 0.0001f && LoadingSystem.LoadingProcess < 0.999f)
        {
            _loadingBar.value = LoadingSystem.LoadingProcess;
            yield return null;
        }
        _loadingBar.value = 1f;

        // 로딩 완료
        // _touchToStartText.SetActive(true);
        // _touchToStartButton.SetActive(true);
        //_eventSystem.SetActive(false);
        HideUI();
    }
    #endregion


    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _touchToStartText.SetActive(false);
        _touchToStartButton.SetActive(false);
    }
}
