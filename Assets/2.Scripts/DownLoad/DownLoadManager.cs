using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DownLoadManager : MonoBehaviour
{
    public GameObject _downLoadWindow;
    public GameObject _completeWindow;
    public TextMeshProUGUI _patchSizeText;

    public List<AssetLabelReference> _labels;

    long patchSize;
    Dictionary<string, long> _patchMap = new Dictionary<string, long>();

    void Start()
    {
        _downLoadWindow.SetActive(false);
        _completeWindow.SetActive(false);
        LoadingSystem.InitializeAddressables(OnAddressableInitComplete);
    }

    void OnAddressableInitComplete()
    {
        Debug.Log("Addressables initialization complete. Starting download check.");
        StartCoroutine(Co_CheckUpdateFiles());
    }

    public void Button_DownLoad()
    {
        StartCoroutine(Co_DownloadAssets());
    }

    public void Button_GoNext()
    {
        LoadingSystem.LoadAddressableLoadingScene();
    }

    IEnumerator Co_CheckUpdateFiles()
    {
        patchSize = 0;

        foreach (var label in _labels)
        {
            var sizeHandle = Addressables.GetDownloadSizeAsync(label.labelString);

            yield return sizeHandle;

            patchSize += sizeHandle.Result;
        }

        if (patchSize > 0)
        {
            Debug.Log("Download required. Total size: " + patchSize + " bytes.");
            string sizeString = FileSizeToString(patchSize);

            _downLoadWindow.SetActive(true);
            _patchSizeText.text = sizeString;
        }
        else
        {
            Debug.Log("No Download");
            _completeWindow.SetActive(true);
        }
    }

    string FileSizeToString(long size)
    {
        if (size < 1024)
            return size + " B";
        else if (size < 1024 * 1024)
            return (size / 1024f).ToString("F2") + " KB";
        else if (size < 1024 * 1024 * 1024)
            return (size / (1024f * 1024f)).ToString("F2") + " MB";
        else
            return (size / (1024f * 1024f * 1024f)).ToString("F2") + " GB";
    }

    IEnumerator Co_DownloadAssets()
    {
        foreach (var label in _labels)
        {
            string labelName = label.labelString;
            _patchMap.Add(labelName, 0);

            var downloadHandle = Addressables.DownloadDependenciesAsync(labelName);

            while (!downloadHandle.IsDone)
            {
                _patchMap[labelName] = downloadHandle.GetDownloadStatus().DownloadedBytes;
                yield return new WaitForEndOfFrame();
            }

            _patchMap[labelName] = downloadHandle.GetDownloadStatus().TotalBytes;
            Addressables.Release(downloadHandle);
        }

        Debug.Log("All downloads completed.");
        _downLoadWindow.SetActive(false);
        _completeWindow.SetActive(true);
    }
}
