using UnityEngine;

public class ShootDirectionController : MonoBehaviour
{
    RectTransform _rect;

    void Start()
    {
        _rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 direction = AimDirectionBridge.AimDirection;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rect.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
