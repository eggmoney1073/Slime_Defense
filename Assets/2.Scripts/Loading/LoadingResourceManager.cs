using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadingResourceManager
{
    public const int _loadingBGCount = 3;
    static int _saveIndex = 0;

    const string _loadingBGAddress = "Assets/8.Texture/Loading/BG/";

    static bool _isLoadingBGLoaded = false;

    static AsyncOperationHandle<Sprite>[] _loadingBGHandles = new AsyncOperationHandle<Sprite>[_loadingBGCount];

    #region [ Loading BG Download ]

    public static void DownLoadAllSprites()
    {
        if (_isLoadingBGLoaded)
            return;

        for (int i = 0; i < _loadingBGCount; i++)
        {
            string address = _loadingBGAddress + "LoadingBG" + (i + 1).ToString() + ".png";
            Addressables.LoadAssetAsync<Sprite>(address).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("Loaded sprite: " + address);
                    _loadingBGHandles[_saveIndex++] = handle;
                }
                else
                {
                    Debug.LogError("Failed to load sprite: " + address);
                }
            };
        }

        _isLoadingBGLoaded = true;
    }

    public static Sprite GetLoadingBG(int index)
    {
        if (index < 0 || index >= _loadingBGCount)
        {
            Debug.LogError("Invalid loading BG index: " + index);
            return null;
        }
        if (_loadingBGHandles[index].IsValid() && _loadingBGHandles[index].Status == AsyncOperationStatus.Succeeded)
        {
            return _loadingBGHandles[index].Result;
        }
        else
        {
            Debug.LogError("Loading BG not loaded or failed to load: " + index);
            return null;
        }
    }

    #endregion
}
