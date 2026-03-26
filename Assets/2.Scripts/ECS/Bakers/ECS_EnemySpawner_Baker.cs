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
            EnemyPrefab = prefab,
            Timer = 0f,
            SpawnInterval = authoring.SpawnInterval,
            SpawnCount = 0,
            MaxSpawnCount = authoring.MaxSpawnCount
        });
    }
}