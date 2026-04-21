using UnityEngine;
using Unity.Entities;

public class LevelUpFlow : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private LevelUpOptionDatabase _database;
    [SerializeField] private LevelUpController _levelUpWindow;
    [SerializeField] private JoystickInput _joystickInput;

    private int _pendingChoices;
    private bool _isShowing;

    private void Awake()
    {
        _levelUpWindow.OnSelectedOptionIndex += OnSelected;
    }

    private void OnDestroy()
    {
        if (_levelUpWindow != null)
            _levelUpWindow.OnSelectedOptionIndex -= OnSelected;
    }

    public void EnqueueLevelUps(int count)
    {
        if (count <= 0) return;

        _pendingChoices += count;

        _levelUpWindow.ActivateWindow();
        _joystickInput.IsPaused = true;
        TimeManager.Instance.SetPause();

        TryShowChoiceWindow();
    }

    private void TryShowChoiceWindow()
    {
        if (_isShowing) return;
        if (_pendingChoices <= 0) return;
        if (_database == null || !_database.IsReady)
        {
            if (!_database.IsReady)
            {
                _database.OnReady -= TryShowChoiceWindow;
                _database.OnReady += TryShowChoiceWindow;
            }
            return;
        }

        _pendingChoices--;
        _isShowing = true;

        int[] picked = LevelUpOptionPicker.Pick3(_database.LevelUpOptionDatas.Length);
        _levelUpWindow.ShowWindow(_database.LevelUpOptionDatas, picked);
    }

    private void OnSelected(int optionIndex)
    {
        _levelUpWindow.HideWindow();
        _isShowing = false;

        SendApplyRequestToECS(optionIndex);

        if (_pendingChoices > 0)
        {
            TryShowChoiceWindow();
        }
        else
        {
            _levelUpWindow.DeactivateWindow();
            _joystickInput.IsPaused = false;
            TimeManager.Instance.Resume();
        }
    }

    private void SendApplyRequestToECS(int optionIndex)
    {
        World world = World.DefaultGameObjectInjectionWorld;
        if (world == null) return;

        EntityManager entityManager = world.EntityManager;
        Entity entity = entityManager.CreateEntity();
        entityManager.AddComponentData(entity, new UpgradeRequest
        {
            optionIndex = optionIndex,
            value = 1 // 나중에 레벨에 따라 달라질 수 있음
        });

        Debug.Log("레벨업 옵션 선택: " + optionIndex);
    }
}
