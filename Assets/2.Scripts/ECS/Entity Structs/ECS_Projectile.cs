using Unity.Entities;
using Unity.Mathematics;

public struct ProjectileTag : IComponentData { }

public struct Direction
{
    public float3 direction;
}

public struct DamageData
{
    public float damage;
}

public struct LifetimeData
{
    public float remainTime;
}

struct PierceData
{
    public int remainCount;
}