using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpOptions : PopUpBase
{
    [Header("Display Settings")]
    [SerializeField]
    TMP_Dropdown _displayResolutionDropdown;
    [SerializeField]
    TextMeshProUGUI _dropDownLabel;
    [SerializeField]
    int _currentResolutionIndex;

    [Header("Sounds Settings")]
    [SerializeField]
    Slider _bgmSlider;
    [SerializeField]
    Text _bgmValue;
    [SerializeField]
    float _currentBGMValue;

    [SerializeField]
    Slider _sfxSlider;
    [SerializeField]
    Text _sfxValue;
    [SerializeField]
    float _currentSFXValue;


    Resolution[] _resolutions;

    public override void Initialize()
    {
        _resolutions = Screen.resolutions;

        _displayResolutionDropdown.options.Clear();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            _displayResolutionDropdown.options.Add(new TMP_Dropdown.OptionData(option));
        }

        _dropDownLabel.text = Screen.currentResolution.width + " x " + Screen.currentResolution.height;
    }

    

    public void Button_Close()
    {
        Hide();
    }

    public void Button_Apply()
    {
        ApplyChanges();
    }

    public void Button_Save()
    {
        ApplyChanges();
        Hide();
    }

    void Start()
    {
        Initialize();
        SetListener();
    }

    void OnDisplayResolutionChanged(int index)
    {
        _currentResolutionIndex = index;
    }

    void OnBGMValueChanged(float value)
    {
        _currentBGMValue = value;
        _bgmValue.text = Mathf.RoundToInt(value * 100).ToString();
    }

    void OnSFXValueChanged(float value)
    {
        _currentSFXValue = value;
        _sfxValue.text = Mathf.RoundToInt(value * 100).ToString();
    }

    public override void Show()
    {
        base.Show();
        SetListener();
    }

    void SetListener()
    {
        if (_displayResolutionDropdown != null)
        {
            _displayResolutionDropdown.onValueChanged.AddListener(OnDisplayResolutionChanged);
        }

        if (_bgmSlider != null)
        {
            _bgmSlider.onValueChanged.AddListener(OnBGMValueChanged);
        }

        if (_sfxSlider != null)
        {
            _sfxSlider.onValueChanged.AddListener(OnSFXValueChanged);
        }
    }

    public override void Hide()
    {
        RemoveListeners();
        base.Hide();
    }

    void RemoveListeners()
    {
        if(_displayResolutionDropdown != null)
        {
            _displayResolutionDropdown.onValueChanged.RemoveListener(OnDisplayResolutionChanged);
        }

        if(_bgmSlider != null)
        {
            _bgmSlider.onValueChanged.RemoveListener(OnBGMValueChanged);
        }

        if(_sfxSlider != null)
        {
            _sfxSlider.onValueChanged.RemoveListener(OnSFXValueChanged);
        }
    }

    void ApplyChanges()
    {
        int resolutionWidth = _resolutions[_currentResolutionIndex].width;
        int resolutionHeight = _resolutions[_currentResolutionIndex].height;

        Screen.SetResolution(resolutionWidth, resolutionHeight, false);
    }
}
