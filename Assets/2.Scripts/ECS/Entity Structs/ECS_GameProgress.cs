using Unity.Entities;
using DefineEnums;

public struct GameProgress : IComponentData
{
    public int kill;
    public float time;
    public int level;
    public int levelUpKillCount;
    public float levelUpPercent;
    public int totalKill;
}

public struct LevelUpEvent : IComponentData
{
    public int levelUpCount;
    public int newLevel;
}

public struct UpgradeRequest : IComponentData
{
    public Entity weaponEntity;
    public float value;
    public WeaponUpgradeType upgradeType;
}

public struct GameOverEvent : IComponentData { }
public struct GameClearEvent : IComponentData { }
