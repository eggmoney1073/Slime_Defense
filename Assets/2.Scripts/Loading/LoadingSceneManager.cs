using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private float _fadeOutDuration = 1f;

    private CanvasGroup _canvasGroup;
    private Action _fadeOutCallBack;

    private Coroutine _loadingBarCoroutine;
    private Coroutine _fadeCoroutine;

    private bool _isHiding;

    #region [ Loading Function ]

    /// <summary>
    /// UI 보이기
    /// </summary>
    public void ShowUI(Action fadeOutCallBack = null)
    {
        EnsureCanvasGroup();

        StopLoadingBarCoroutine();
        StopFadeCoroutine();

        SetRandomBG();

        _fadeOutCallBack = fadeOutCallBack;
        _isHiding = false;

        if (_titleImage != null)
        {
            _titleImage.SetActive(false);
        }

        if (_touchToStartText != null)
        {
            _touchToStartText.SetActive(false);
        }

        _loadingBar.value = 0f;

        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;

        _loadingBarCoroutine = StartCoroutine(Co_SetLoadingBar());
    }

    private void SetRandomBG()
    {
        if (_loadingBGImage == null)
        {
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, LoadingResourceManager._loadingBGCount);
        _loadingBGImage.sprite = LoadingResourceManager.GetLoadingBG(randomIndex);
    }

    public void HideUI()
    {
        RequestHideUI(true);
    }

    private void RequestHideUI(bool stopLoadingCoroutine)
    {
        EnsureCanvasGroup();

        if (_isHiding)
        {
            return;
        }

        _isHiding = true;

        if (stopLoadingCoroutine)
        {
            StopLoadingBarCoroutine();
        }

        StopFadeCoroutine();

        _fadeCoroutine = StartCoroutine(Co_FadeOut(_fadeOutDuration));
    }

    private IEnumerator Co_FadeOut(float duration)
    {
        float elapsed = 0f;
        float startAlpha = _canvasGroup.alpha;
        float fadeDuration = Mathf.Max(duration, 0.01f);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / fadeDuration);
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);

            yield return null;
        }

        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;

        _fadeCoroutine = null;

        Action callBack = _fadeOutCallBack;
        _fadeOutCallBack = null;

        callBack?.Invoke();
    }

    private IEnumerator Co_SetLoadingBar()
    {
        float minLoadingTime = Mathf.Max(_minLoadingTime, 0.01f);
        float minTimeDelta = 1f / minLoadingTime;

        float loadingBarValue = 0f;
        float timeValue = 0f;

        while (loadingBarValue < 1f && !_isHiding)
        {
            timeValue += minTimeDelta * Time.unscaledDeltaTime;
            timeValue = Mathf.Clamp01(timeValue);

            float processValue = Mathf.Clamp01(LoadingSystem.LoadingProcess);
            float nextValue = Mathf.Min(processValue, timeValue);

            if (nextValue > loadingBarValue)
            {
                loadingBarValue = nextValue;
                _loadingBar.value = loadingBarValue;
            }

            yield return null;
        }

        if (!_isHiding)
        {
            _loadingBar.value = 1f;
            _loadingBarCoroutine = null;

            RequestHideUI(false);
        }
    }

    #endregion

    private void Start()
    {
        EnsureCanvasGroup();

        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;

        if (_loadingBar != null)
        {
            _loadingBar.value = 0f;
        }
    }

    private void EnsureCanvasGroup()
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    private void StopLoadingBarCoroutine()
    {
        if (_loadingBarCoroutine != null)
        {
            StopCoroutine(_loadingBarCoroutine);
            _loadingBarCoroutine = null;
        }
    }

    private void StopFadeCoroutine()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }
    }
}