using Unity.Entities;

public struct GameProgress : IComponentData
{
    public int kill;
    public float time;
    public int level;
    public int levelUpKillCount;
    public float levelUpPercent;
}

public struct LevelUpEvent : IComponentData
{
    public int levelUpCount;
    public int newLevel;
}

public struct UpgradeRequest : IComponentData
{
    public float value;
    public int optionIndex;
}
