using UnityEngine;
using Unity.Entities;

public class LevelUpFlow : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private LevelUpController _levelUpWindow;
    [SerializeField] private JoystickInput _joystickInput;

    [Header("설정")]
    [SerializeField] private string _databaseTag = "GameDatabase";
    private LevelUpOptionDatabase _levelupOptionDatabase;
    private WeaponDatabase _weaponDatabase;
    private int _pendingChoices;
    private bool _isShowing;

    private void Awake()
    {
        _levelUpWindow.OnSelectedOptionIndex += OnSelected;

        GameObject databaseObject = GameObject.FindWithTag(_databaseTag);
        if (databaseObject == null)
        {
            Debug.LogError($"Tag '{_databaseTag}'를 가진 GameObject를 찾을 수 없습니다.");
            return;
        }
        _levelupOptionDatabase = databaseObject.GetComponent<LevelUpOptionDatabase>();
        _weaponDatabase = databaseObject.GetComponent<WeaponDatabase>();
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
        GameFlowManager.Instance.ChangeGameState(GameFlowManager.GameState.Pause);

        TryShowChoiceWindow();
    }

    private void TryShowChoiceWindow()
    {
        if (_isShowing) return;
        if (_pendingChoices <= 0) return;
        if (_levelupOptionDatabase == null || !_levelupOptionDatabase.IsReady)
        {
            if (!_levelupOptionDatabase.IsReady)
            {
                _levelupOptionDatabase.OnReady -= TryShowChoiceWindow;
                _levelupOptionDatabase.OnReady += TryShowChoiceWindow;
            }
            return;
        }

        _pendingChoices--;
        _isShowing = true;

        int[] picked = LevelUpOptionPicker.Pick3(_levelupOptionDatabase.LevelUpOptionDatas.Length);
        _levelUpWindow.ShowWindow(_levelupOptionDatabase.LevelUpOptionDatas, picked);
    }

    private void OnSelected(int optionIndex)
    {
        _levelUpWindow.HideWindow();
        _isShowing = false;

        SendApplyRequestToECS(_levelupOptionDatabase.LevelUpOptionDatas[optionIndex]);

        if (_pendingChoices > 0)
        {
            TryShowChoiceWindow();
        }
        else
        {
            _levelUpWindow.DeactivateWindow();
            _joystickInput.IsPaused = false;
            GameFlowManager.Instance.ChangeGameState(GameFlowManager.GameState.Playing);
        }
    }

    private void SendApplyRequestToECS(LevelUpOptionData optionData)
    {
        World world = World.DefaultGameObjectInjectionWorld;
        if (world == null) return;

        EntityManager entityManager = world.EntityManager;
        Entity entity = entityManager.CreateEntity();
        entityManager.AddComponentData(entity, new UpgradeRequest
        {
            weaponEntity = _weaponDatabase.GetWeaponEntity(optionData.weaponType),
            upgradeType = optionData.upgradeType,
            value = optionData.value
        });
    }
}
