using TMPro;
using UnityEngine;

public class EnemyCount_Mono : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _countText;

    void Update()
    {
        _countText.text = EnemyCount.Count.ToString();
    }
}
