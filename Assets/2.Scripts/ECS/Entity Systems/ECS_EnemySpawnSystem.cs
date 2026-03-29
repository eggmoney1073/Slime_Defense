using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct ECS_EnemySpawnSystem : ISystem
{
    EntityQuery enemyQuery;

    public void OnCreate(ref SystemState state)
    {
        enemyQuery = state.GetEntityQuery(ComponentType.ReadOnly<EnemyTag>());
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

        foreach (RefRW<ECS_EnemySpawner> spawner in SystemAPI.Query<RefRW<ECS_EnemySpawner>>())
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

                    // 위치 설정
                    ecb.SetComponent(enemy, LocalTransform.FromPosition(spawnPos));

                    // Path 연결
                    ecb.AddComponent(enemy, new ECS_PathReference
                    {
                        path = pathEntity
                    });

                    spawner.ValueRW.spawnCount += 1;
                }
            }
        }

        // 마지막에 적용
        ecb.Playback(state.EntityManager);
        ecb.Dispose();

        EnemyCount.SetCount(enemyQuery.CalculateEntityCount());
    }

}
