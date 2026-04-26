using Unity.VisualScripting;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    [SerializeField] private GameObject _timeActiveImage;

    bool _isDoubleSpeed;

    void Start()
    {
        _timeActiveImage.SetActive(false);
        _isDoubleSpeed = false;
    }

    public void OnTimeScaleButtonClicked()
    {
        if (_isDoubleSpeed)
        {
            TimeManager.Instance.SetTimeScaleNormal();
            _timeActiveImage.SetActive(false);
            _isDoubleSpeed = false;
        }
        else
        {
            TimeManager.Instance.SetTimeScaleFast();
            _timeActiveImage.SetActive(true);
            _isDoubleSpeed = true;
        }
    }
}
