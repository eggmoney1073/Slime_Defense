using Unity.Entities;

public struct EnemyTag : IComponentData { }

public struct EnemyWayPoint : IComponentData
{
    public int currentIndex;
}

public struct EnemyHealth : IComponentData 
{
    public float health;
}

public struct Damaged : IBufferElementData
{
    public float damage;
}