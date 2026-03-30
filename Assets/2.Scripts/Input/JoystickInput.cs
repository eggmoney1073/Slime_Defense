using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickInput : MonoBehaviour
{
    [SerializeField]
    GameObject _joystickPrefab;

    RectTransform _joystickRoot;
    RectTransform _joystick;

    Canvas _inputCanvas;
    Camera _uiCamera;

    Vector2 _direction = Vector2.zero;

    Vector2 _touchStartPosition = Vector2.zero;


    void Awake()
    {
        _inputCanvas = GetComponent<Canvas>();

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

        // 터치 시작
        if (touch.press.wasPressedThisFrame)
        {
            _joystickRoot.gameObject.SetActive(true);

            Vector2 screenPos = touch.position.ReadValue();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_inputCanvas.transform as RectTransform, screenPos, _uiCamera, out Vector2 localPos);

            _joystickRoot.anchoredPosition = localPos;
            _touchStartPosition = localPos;
        }

        // 터치 중
        if(touch.press.isPressed)
        {
            Vector2 screenPos = touch.position.ReadValue();
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
}