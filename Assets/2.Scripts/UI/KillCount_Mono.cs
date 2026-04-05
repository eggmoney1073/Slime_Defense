using TMPro;
using UnityEngine;

public class KillCount_Mono : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _countText;

    void Update()
    {
        _countText.text = KillCount.Kill.ToString();
    }
}
