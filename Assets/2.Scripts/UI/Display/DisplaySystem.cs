using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using DefineEnums;

public class DisplaySystem
{
    Dictionary<UIType, GameObject> _displayUIInstances = new Dictionary<UIType, GameObject>();

    private bool _isInitialized = false;
    string _displayUIAddressBasePath;

    public void InitializeSystem()
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
            _isInitialized = true;
            Addressables.Release(handle);
        };
    }

    public void InstantiateDisplayUI(UIType displayUIType, Transform uiRoot)
    {
        if (!_isInitialized)
        {
            InitializeSystem();
        }

        string path = _displayUIAddressBasePath + displayUIType.ToString() + ".prefab";
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
