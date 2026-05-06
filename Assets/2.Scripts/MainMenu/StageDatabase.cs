using UnityEngine;

public class StageDatabase : MonoBehaviour
{
    [SerializeField] private Sprite[] StageImages;
    private int _currentStageIndex = 0;
    private bool[] _openStages;

    public int CurrentStageIndex { get { return _currentStageIndex; } set { _currentStageIndex = value; } }
    public int TotalStages => StageImages.Length;

    public bool GetStageOpen(int index)
    {
        if (index < 0 || index >= _openStages.Length)
        {
            Debug.LogError($"인덱스 {index}가 범위를 벗어났습니다.");
            return false;
        }
        return _openStages[index];
    }

    public Sprite GetStageImage(int index)
    {
        if (index < 0 || index >= StageImages.Length)
        {
            Debug.LogError($"인덱스 {index}가 범위를 벗어났습니다.");
            return null;
        }
        return StageImages[index];
    }

    public void Initialize(int totalStages)
    {
        _openStages = new bool[totalStages];
        _openStages[0] = true;
    }

    private void Start()
    {
        // 임시로 2개
        Initialize(2);
    }
}
