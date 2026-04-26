using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _weaponNameText;
    [SerializeField] private TextMeshProUGUI _informationText;
    [SerializeField] private Button _choiceButton;

    public void SetChoice(int optionIndex, LevelUpOptionData data, Action<int> onButtonClicked)
    {
        _weaponNameText.text = data.weaponType.ToString();
        _informationText.text = data.description;

        _choiceButton.onClick.RemoveAllListeners();
        _choiceButton.onClick.AddListener(() =>
        {
            onButtonClicked.Invoke(optionIndex);
        });
    }
}
