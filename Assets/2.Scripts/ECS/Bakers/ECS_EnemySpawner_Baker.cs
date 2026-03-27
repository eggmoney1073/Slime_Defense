using Unity.Entities;
using UnityEngine;

class ECS_EnemySpawner_Baker : Baker<EnemySpawner_Authoring>
{
    public override void Bake(EnemySpawner_Authoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        Entity prefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic);

        AddComponent(entity, new ECS_EnemySpawner
        {
            enemyPrefab = prefab,
            timer = 0f,
            spawnInterval = authoring.SpawnInterval,
            spawnCount = 0,
            maxSpawnCount = authoring.MaxSpawnCount
        });
    }
}