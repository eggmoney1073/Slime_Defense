using Unity.Entities;

public struct GameProgress : IComponentData
{
    public float exp;
    public int kill;
    public float time;
}
