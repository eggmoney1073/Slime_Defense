using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using DefineEnums;
using System;

public class LevelUpController : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private RectTransform[] _slotPositions;
    [SerializeField] private GameObject[] _slotPrefabs;
    [SerializeField] private GameObject _choiceWindow;

    private ChoiceController[] _choices;
    public event Action<int> OnSelectedOptionIndex;

    private void Awake()
    {
        _choices = new ChoiceController[3];
        gameObject.SetActive(false);
    }

    public void ActivateWindow()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateWindow()
    {
        for (int i = 0; i < _choices.Length; i++)
        {
            Destroy(_choices[i].gameObject);
        }

        gameObject.SetActive(false);
    }

    public void ShowWindow(LevelUpOptionData[] optionDatas, int[] pickedIndexes)
    {
        _choiceWindow.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            int index = pickedIndexes[i];
            LevelUpOptionData data = optionDatas[index];

            // 선택 실패
            if (index < 0 || index >= optionDatas.Length)
            {
                Debug.LogError(index.ToString() + "값이 범위에 없음");
                return;
            }

            GameObject instance = Instantiate(_slotPrefabs[(int)data.rarity], _slotPositions[i]);
            ChoiceController choiceController = instance.GetComponent<ChoiceController>();
            choiceController.SetChoice(index, data, OnSelectedOptionIndex);
            _choices[i] = choiceController;
        }
    }

    public void HideWindow()
    {
        _choiceWindow.SetActive(false);
    }
}
