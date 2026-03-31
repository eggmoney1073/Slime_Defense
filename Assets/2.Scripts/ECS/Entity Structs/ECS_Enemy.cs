using Unity.Entities;

public struct EnemyTag : IComponentData { }

public struct EnemyDeadTag : IComponentData , IEnableableComponent { }

public struct EnemyHealth : IComponentData 
{
    public float Health;
}
