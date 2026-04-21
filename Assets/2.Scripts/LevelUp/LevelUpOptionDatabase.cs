using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelUpOptionDatabase : MonoBehaviour
{
    [Header("Addressables 설정")]
    [SerializeField] private string _basePath = "Assets/6.Data/Upgrade/MainWeapon/Option";
    [SerializeField] private int _optionCount = 10;

    public bool IsReady { get; private set; }
    public LevelUpOptionData[] LevelUpOptionDatas { get; private set; }
    public event Action OnReady;

    private void Awake()
    {
        IsReady = false;
        LevelUpOptionDatas = new LevelUpOptionData[_optionCount];
        LoadLevelUpOptionData();
    }

    private void LoadLevelUpOptionData()
    {
        int loadedCount = 0;

        LevelUpOptionDatas = new LevelUpOptionData[_optionCount];

        for (int i = 0; i < _optionCount; i++)
        {
            int choiceIndex = i;
            string path = _basePath + (i + 1).ToString() + ".asset";
            AsyncOperationHandle<LevelUpOptionData> handle = Addressables.LoadAssetAsync<LevelUpOptionData>(path);
            handle.Completed += CompletedHandle =>
            {
                if (CompletedHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogErrorFormat("{0} 번째 Scriptable asset 로드 실패");
                }
                else
                {
                    LevelUpOptionDatas[choiceIndex] = handle.Result;
                }

                loadedCount++;

                if (loadedCount >= _optionCount)
                {
                    IsReady = true;
                    OnReady?.Invoke();
                }
            };
        }
    }
}
