using UnityEngine;

public class DisplayUIManager : MonoBehaviour
{
    [SerializeField] private Transform _safeZone;
    private DisplaySystem _displaySystem = new DisplaySystem();

    // ui 모두 instantiate
    void Awake()
    {
        _displaySystem.InstantiateDisplayUI(DisplaySystem.DisplayUIType.EXP, _safeZone);
        _displaySystem.InstantiateDisplayUI(DisplaySystem.DisplayUIType.Timer, _safeZone);
        _displaySystem.InstantiateDisplayUI(DisplaySystem.DisplayUIType.ShootDirection, _safeZone);
    }

    // ui 모두 release
    void OnDestroy()
    {
        _displaySystem.ReleaseAllDisplayUI();
    }
}
