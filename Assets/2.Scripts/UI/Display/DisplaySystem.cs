using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using DefineEnums;

public class DisplaySystem
{
    Dictionary<UIType, GameObject> _displayUIInstances = new Dictionary<UIType, GameObject>();

    string _displayUIAddressBasePath;

    public void InstantiateAllDisplayUI(Transform uiRoot)
    {
        AsyncOperationHandle<AddressablePath> handle = Addressables.LoadAssetAsync<AddressablePath>("Assets/6.Data/DisplayUIPath.asset");
        handle.Completed += CompletedHandle =>
        {
            if (CompletedHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("DisplaySystem 초기화 실패: AddressablePath 로드 실패");
                return;
            }

            _displayUIAddressBasePath = CompletedHandle.Result.PrefabAddress;
            Addressables.Release(handle);

            InstantiateDisplayUI(UIType.EXP, uiRoot);
            InstantiateDisplayUI(UIType.Timer, uiRoot);
            InstantiateDisplayUI(UIType.ShootDirection, uiRoot);
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

    private void InstantiateDisplayUI(UIType displayUIType, Transform uiRoot)
    {
        string path = _displayUIAddressBasePath + displayUIType.ToString() + ".prefab";
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(path, uiRoot);

        handle.Completed += CompletedHandle =>
        {
            OnPrefabLoaded(CompletedHandle, displayUIType);
        };
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle, UIType displayUIType)
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
