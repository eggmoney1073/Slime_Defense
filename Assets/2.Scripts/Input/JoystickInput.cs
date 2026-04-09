using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickInput : MonoBehaviour
{
    [SerializeField] private GameObject _joystickPrefab;
    [SerializeField] private Canvas _inputCanvas;

    bool _isTouching = false;
    RectTransform _joystickRoot;
    RectTransform _joystick;
    RectTransform _joystickArea;

    Camera _uiCamera;
    Vector2 _direction = Vector2.zero;

    void Awake()
    {
        _joystickArea = transform.GetComponent<RectTransform>();
        _uiCamera = _inputCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _inputCanvas.worldCamera;
    }

    void Start()
    {
        GameObject joystick = Instantiate(_joystickPrefab, _inputCanvas.transform);

        _joystickRoot = joystick.GetComponent<RectTransform>();
        _joystick = _joystickRoot.GetChild(1).GetComponent<RectTransform>();

        _joystickRoot.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Touchscreen.current == null)
            return;

        var touch = Touchscreen.current.primaryTouch;
        Vector2 screenPos = touch.position.ReadValue();

        // 터치 시작
        if (touch.press.wasPressedThisFrame)
        {
            if (!IsInTouchArea(screenPos))
            {
                _isTouching = false;
                return;
            }

            _isTouching = true;
            _joystickRoot.gameObject.SetActive(true);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_inputCanvas.transform as RectTransform, screenPos, _uiCamera, out Vector2 localPos);

            _joystickRoot.anchoredPosition = localPos;
        }

        // 터치 중
        if (_isTouching && touch.press.isPressed)
        {
            if (!IsInTouchArea(screenPos))
            {
                _isTouching = false;
                _joystickRoot.gameObject.SetActive(false);
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickRoot.transform as RectTransform, screenPos, _uiCamera, out Vector2 localPos);
            _joystick.anchoredPosition = localPos;

            // 방향
            _direction = localPos.normalized;
            AimDirectionBridge.SetAimDirection(_direction.x, _direction.y);

            //Debug.Log(_direction);
        }


        // 터치 끝
        if (touch.press.wasReleasedThisFrame)
        {
            _joystickRoot.gameObject.SetActive(false);
        }
    }

    bool IsInTouchArea(Vector2 screenPos)
    {
        if (_joystickArea == null) return false;

        return RectTransformUtility.RectangleContainsScreenPoint(_joystickArea, screenPos, _uiCamera);
    }
}