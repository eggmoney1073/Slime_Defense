using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _stageNameText;
    [SerializeField] private TextMeshProUGUI _stageDescriptionText;
    [SerializeField] private Image _stageImage;
    [SerializeField] private Image _lockedImage;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private StageDatabase _stageDatabase;

    public void Initialize()
    {
        SetStageInfo(_stageDatabase.CurrentStageIndex);
        CheckSideStage(_stageDatabase.CurrentStageIndex);
        _nextButton.onClick.AddListener(NextStage);
        _previousButton.onClick.AddListener(PreviousStage);
    }

    public void NextStage()
    {
        int nextIndex = _stageDatabase.CurrentStageIndex + 1;
        if (nextIndex < _stageDatabase.TotalStages)
        {
            _stageDatabase.CurrentStageIndex = nextIndex;
            SetStageInfo(nextIndex);
            CheckSideStage(nextIndex);
        }
    }

    public void PreviousStage()
    {
        int previousIndex = _stageDatabase.CurrentStageIndex - 1;
        if (previousIndex >= 0)
        {
            _stageDatabase.CurrentStageIndex = previousIndex;
            SetStageInfo(previousIndex);
            CheckSideStage(previousIndex);
        }
    }

    private void CheckSideStage(int stageIndex)
    {
        if (stageIndex == 0)
        {
            _previousButton.interactable = false;
            _nextButton.interactable = true;
        }
        else if (stageIndex == _stageDatabase.TotalStages - 1)
        {
            _previousButton.interactable = true;
            _nextButton.interactable = false;
        }
        else
        {
            _previousButton.interactable = true;
            _nextButton.interactable = true;
        }
    }

    private void SetStageInfo(int stageIndex)
    {
        _stageNameText.text = "Stage " + (stageIndex + 1).ToString();
        _stageDescriptionText.text = _stageDatabase.GetStageOpen(stageIndex) ? "Opened" : "Locked";
        _stageImage.sprite = _stageDatabase.GetStageImage(stageIndex);
        _lockedImage.enabled = !_stageDatabase.GetStageOpen(stageIndex);
        _startButton.interactable = _stageDatabase.GetStageOpen(stageIndex);
    }

    private void Start()
    {
        Initialize();
    }
}
