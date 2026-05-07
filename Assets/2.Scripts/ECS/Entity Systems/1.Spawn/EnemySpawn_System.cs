using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(SpawnSystemGroup))]
partial struct EnemySpawn_System : ISystem
{
    EntityQuery _enemyQuery;
    bool isClear;
    int _spawnIndex;

    public void OnCreate(ref SystemState state)
    {
        _enemyQuery = state.GetEntityQuery(ComponentType.ReadOnly<EnemyTag>());
        _spawnIndex = 0;
        isClear = false;
    }

    public void OnUpdate(ref SystemState state)
    {
        TimeScaleData timeData = SystemAPI.GetSingleton<TimeScaleData>();
        float deltaTime = timeData.scaledDeltaTime;

        if (!SystemAPI.TryGetSingletonEntity<PathTag>(out Entity pathEntity))
        {
            return;
        }

        DynamicBuffer<ECS_WayPoint> waypoints = SystemAPI.GetBuffer<ECS_WayPoint>(pathEntity);

        if (waypoints.Length == 0)
        {
            return;
        }

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        bool hasSpawner = false;
        bool isAllSpawnFinished = true;

        foreach (RefRW<EnemySpawner> spawner in SystemAPI.Query<RefRW<EnemySpawner>>())
        {
            hasSpawner = true;

            if (spawner.ValueRO.spawnCount < spawner.ValueRO.maxSpawnCount)
            {
                isAllSpawnFinished = false;

                spawner.ValueRW.timer += deltaTime;

                if (spawner.ValueRO.timer >= spawner.ValueRO.spawnInterval)
                {
                    spawner.ValueRW.timer = 0f;

                    Entity enemy = ecb.Instantiate(spawner.ValueRO.enemyPrefab);

                    float3 spawnPos = waypoints[0].nodePosition;
                    spawnPos.z += _spawnIndex * 0.001f;

                    ecb.SetComponent(enemy, LocalTransform.FromPosition(spawnPos));

                    ecb.AddComponent(enemy, new EntityIndex
                    {
                        index = _spawnIndex
                    });

                    _spawnIndex++;

                    ecb.AddComponent(enemy, new ECS_PathReference
                    {
                        path = pathEntity
                    });

                    ecb.AddComponent(enemy, new EnemyHealth
                    {
                        health = 100f + (_spawnIndex * 100f * spawner.ValueRO.enemyHealthSale)
                    });

                    ecb.AddBuffer<Damaged>(enemy);

                    spawner.ValueRW.spawnCount += 1;
                }
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();

        if (!isClear && hasSpawner && isAllSpawnFinished)
        {
            int aliveEnemyCount = _enemyQuery.CalculateEntityCount();

            if (aliveEnemyCount == 0)
            {
                Entity clearEntity = state.EntityManager.CreateEntity();
                state.EntityManager.AddComponent<GameClearEvent>(clearEntity);

                isClear = true;
            }
        }
    }
}