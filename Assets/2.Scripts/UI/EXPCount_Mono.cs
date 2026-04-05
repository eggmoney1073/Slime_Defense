using TMPro;
using UnityEngine;

public class EXPCount_Mono : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _countText;

    void Update()
    {
        _countText.text = EXPCount.Exp.ToString();
    }
}
