using Unity.Entities;

public struct ECS_EnemySpawner : IComponentData
{
    public Entity enemyPrefab;
    public float timer;
    public float spawnInterval;
    public int spawnCount;
    public int maxSpawnCount;
}
