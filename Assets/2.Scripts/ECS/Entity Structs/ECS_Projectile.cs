using Unity.Entities;
using Unity.Mathematics;

public struct ProjectileTag : IComponentData { }

public struct Direction : IComponentData
{
    public float3 moveDirection;
}

public struct ProjectileDamage : IComponentData
{
    public float damage;
}

public struct LifetimeData : IComponentData
{
    public float remainTime;
}

public struct PierceData : IComponentData
{
    public int maxPierceCount;
}

public struct HitEnemyBufferElement : IBufferElementData
{
    public Entity enemy;
}