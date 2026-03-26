using Unity.Entities;
using Unity.Mathematics;

public struct ProjectileTag : IComponentData { }

public struct Direction : IComponentData
{
    public float3 direction;
}

public struct DamageData : IComponentData
{
    public float damage;
}

public struct LifetimeData : IComponentData
{
    public float remainTime;
}

struct PierceData : IComponentData
{
    public int remainCount;
}