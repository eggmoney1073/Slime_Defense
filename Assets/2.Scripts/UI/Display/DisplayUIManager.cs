using UnityEngine;
using DefineEnums;

public class DisplayUIManager : MonoBehaviour
{
    [SerializeField] private Transform _safeZone;
    private DisplaySystem _displaySystem = new DisplaySystem();

    // ui 모두 instantiate
    void Awake()
    {
        _displaySystem.InstantiateDisplayUI(UIType.EXP, _safeZone);
        _displaySystem.InstantiateDisplayUI(UIType.Timer, _safeZone);
        _displaySystem.InstantiateDisplayUI(UIType.ShootDirection, _safeZone);
    }

    // ui 모두 release
    void OnDestroy()
    {
        _displaySystem.ReleaseAllDisplayUI();
    }
}
