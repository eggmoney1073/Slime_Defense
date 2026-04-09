using UnityEngine;
using DefineEnums;

public class DisplayUIManager : MonoBehaviour
{
    [SerializeField] private Transform _safeZone;
    private DisplaySystem _displaySystem = new DisplaySystem();

    // ui 모두 instantiate

    void Awake()
    {
        _displaySystem.InstantiateAllDisplayUI(_safeZone);
    }

    // ui 모두 release
    void OnDestroy()
    {
        _displaySystem.ReleaseAllDisplayUI();
    }
}
