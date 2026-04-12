using TMPro;
using UnityEngine;

public class ChoiceController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _weaponNameText;
    [SerializeField] private TextMeshProUGUI _informationText;

    LevelUpController _levelUpController;

    public void SetText(LevelUpOptionData data, LevelUpController levelUpController)
    {
        _weaponNameText.text = data.Type.ToString();
        _informationText.text = data.description;
        _levelUpController = levelUpController;
    }

    public void OnChoiceSlot()
    {

    }
}
