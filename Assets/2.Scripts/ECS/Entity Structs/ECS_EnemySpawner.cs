using Unity.Entities;

public struct ECS_EnemySpawner : IComponentData
{
    public Entity EnemyPrefab;
    public float Timer;
    public float SpawnInterval;
    public int SpawnCount;
    public int MaxSpawnCount;
}
