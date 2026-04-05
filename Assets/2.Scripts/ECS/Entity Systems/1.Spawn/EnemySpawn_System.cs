using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[BurstCompile]
[UpdateInGroup(typeof(SpawnSystemGroup))]
partial struct EnemySpawn_System : ISystem
{
    EntityQuery _enemyQuery;
    int _spawnIndex;

    public void OnCreate(ref SystemState state)
    {
        _enemyQuery = state.GetEntityQuery(ComponentType.ReadOnly<EnemyTag>());
        _spawnIndex = 0;
    }

    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        // path 안전하게 가져오기
        if (!SystemAPI.TryGetSingletonEntity<PathTag>(out Entity pathEntity))
        {
            return;
        }

        DynamicBuffer<ECS_WayPoint> waypoints = SystemAPI.GetBuffer<ECS_WayPoint>(pathEntity);

        if (waypoints.Length == 0)
        {
            return;
        }

        // 버퍼 생성
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (RefRW<EnemySpawner> spawner in SystemAPI.Query<RefRW<EnemySpawner>>())
        {
            if (spawner.ValueRO.spawnCount < spawner.ValueRO.maxSpawnCount)
            {
                spawner.ValueRW.timer += deltaTime;

                if (spawner.ValueRO.timer >= spawner.ValueRO.spawnInterval)
                {
                    spawner.ValueRW.timer = 0f;

                    // Entity 생성 (ECB)
                    Entity enemy = ecb.Instantiate(spawner.ValueRO.enemyPrefab);

                    float3 spawnPos = waypoints[0].nodePosition;
                    spawnPos.z += _spawnIndex * 0.001f;

                    // 위치 설정
                    ecb.SetComponent(enemy, LocalTransform.FromPosition(spawnPos));

                    ecb.AddComponent(enemy, new EntityIndex
                    {
                        index = _spawnIndex
                    });
                    _spawnIndex++;

                    // Path 연결
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

        // 마지막에 적용
        ecb.Playback(state.EntityManager);
        ecb.Dispose();

        EnemyCount.SetCount(_enemyQuery.CalculateEntityCount());
    }

}
