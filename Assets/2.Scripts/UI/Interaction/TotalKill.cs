using UnityEngine;
using TMPro;

public class TotalKill : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalKillText;

    private void OnEnable()
    {
        _totalKillText.text = KillCount.Kill.ToString();
    }
}
