using TMPro;
using UnityEngine;

public class TimeCount_Mono : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _countText;

    void Update()
    {
        _countText.text = TimeCount.Time.ToString();
    }
}
