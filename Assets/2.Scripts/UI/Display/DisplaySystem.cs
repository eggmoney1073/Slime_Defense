using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DisplaySystem
{
    public enum DisplayUIType
    {
        EXP,
        Timer,
        ShootDirection
    }


    Dictionary<DisplayUIType, GameObject> _displayUIInstances = new Dictionary<DisplayUIType, GameObject>();

    const string _displayUIAddressBasePath = "4.Prefabs/UI/Display/";

    public void InstantiateDisplayUI(DisplayUIType displayUIType, Transform uiRoot)
    {
        string path = _displayUIAddressBasePath + displayUIType.ToString();
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(path, uiRoot);

        handle.Completed += CompletedHandle =>
        {
            OnPrefabLoaded(CompletedHandle, displayUIType);
        };
    }

    public void ReleaseAllDisplayUI()
    {
        foreach (var kvp in _displayUIInstances)
        {
            Addressables.ReleaseInstance(kvp.Value);
        }
        _displayUIInstances.Clear();
    }

    private void ReleaseDisplayUI(DisplayUIType displayUIType)
    {
        if (_displayUIInstances.TryGetValue(displayUIType, out GameObject instance))
        {
            Addressables.ReleaseInstance(instance);
            _displayUIInstances.Remove(displayUIType);
        }
        else
        {
            Debug.LogWarning($"Display UI of type {displayUIType} not found for release.");
        }
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle, DisplayUIType displayUIType)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("UI 프리팹 로드 실패");
            return;
        }

        GameObject instance = handle.Result;
        _displayUIInstances.Add(displayUIType, instance);
    }


}
