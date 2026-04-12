using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using DefineEnums;

public class LevelUpController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private RectTransform[] _slotPositions;
    [SerializeField] private GameObject[] _slotPrefabs;

    [Header("세팅")]
    [SerializeField] private int _choiceCount = 10;

    private LevelUpOptionData[] _levelUpOptionDatas;
    private ChoiceController[] _choices;



    private const string _basePath = "Assets/6.Data/Upgrade/MainWeapon/Option";

    private void Awake()
    {
        _choices = new ChoiceController[3];
        _levelUpOptionDatas = new LevelUpOptionData[_choiceCount];

        for (int i = 0; i < _choiceCount; i++)
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
                    _levelUpOptionDatas[choiceIndex] = handle.Result;
                }
            };
        }

    }

    public void ShowUI()
    {
        gameObject.SetActive(true);

        int[] indexes = GetRandom3Choice();

        for (int i = 0; i < 3; i++)
        {
            int index = indexes[i];
            LevelUpOptionData data = _levelUpOptionDatas[index];

            // 선택 실패
            if (index < 0 || index >= 10)
            {
                Debug.Log(index.ToString() + "값이 범위에 없음");
                return;
            }

            // 선택지가 Common 이면 (0,1,2,3)
            if (data.rarity == SelectionRarity.Common)
            {
                GameObject instance = Instantiate(_slotPrefabs[(int)SelectionRarity.Common], _slotPositions[i]);
                InitSlot(i, data, instance);
            }
            // 선택지가 Rare 이면 (4,5,6,7)
            else if (data.rarity == SelectionRarity.Rare)
            {
                GameObject instance = Instantiate(_slotPrefabs[(int)SelectionRarity.Rare], _slotPositions[i]);
                InitSlot(i, data, instance);
            }
            // 선택지가 Epic 이면 (8,9)
            else if (data.rarity == SelectionRarity.Epic)
            {
                GameObject instance = Instantiate(_slotPrefabs[(int)SelectionRarity.Epic], _slotPositions[i]);
                InitSlot(i, data, instance);
            }
        }
    }

    public void HideUI()
    {
        // destory slot
        for (int i = 0; i < _choices.Length; i++)
        {
            Destroy(_choices[i]);
        }

        gameObject.SetActive(false);

    }

    private void InitSlot(int slotindex, LevelUpOptionData data, GameObject slotInstance)
    {
        ChoiceController choiceController = slotInstance.GetComponent<ChoiceController>();
        choiceController.SetText(data, this);
        _choices[slotindex] = choiceController;
    }

    /// <summary>
    /// 겹치지 않게 랜덤으로 int형 3개를 출력하는 함수 (0~9 까지)
    /// </summary>
    /// <returns></returns>
    private int[] GetRandom3Choice()
    {
        int[] pool = new int[9];
        int[] result = new int[3];

        for (int i = 0; i < 9; i++)
        {
            pool[i] = i;
        }

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(i, 9);

            int temp = pool[i];
            pool[i] = pool[randomIndex];
            pool[randomIndex] = temp;

            result[i] = pool[i];
        }

        return result;
    }

}
