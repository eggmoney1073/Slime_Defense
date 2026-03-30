using TMPro;
using UnityEngine;

public class ProjectileCount_Mono : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _countText;

    void Update()
    {
        _countText.text = ProjectileCount.Count.ToString();
    }
}
