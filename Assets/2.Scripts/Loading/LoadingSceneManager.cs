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
    [Header("로딩 씬 UI 참조")]
    [SerializeField] private Image _loadingBGImage;
    [SerializeField] private Slider _loadingBar;
    [SerializeField] private GameObject _titleImage;
    [SerializeField] private GameObject _touchToStartText;

    [Header("Settings")]
    [SerializeField] private float _minLoadingTime = 2f;

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
        float minTimeDelta = 1f / _minLoadingTime;
        float lodingBarValue = 0f;
        float timeValue = 0f;

        while (lodingBarValue < 1f)
        {
            timeValue += minTimeDelta * Time.deltaTime;
            lodingBarValue = Mathf.Min(LoadingSystem.LoadingProcess, timeValue);
            _loadingBar.value = lodingBarValue;
            yield return null;
        }

        _loadingBar.value = 1f;

        HideUI();
    }
    #endregion


    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
    }
}
