using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct ECS_EnemySpawnSystem : ISystem
{
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

        // ECB 생성
        EntityCommandBuffer ecb =
            new EntityCommandBuffer(Allocator.Temp);

        foreach (RefRW<ECS_EnemySpawner> spawner in SystemAPI.Query<RefRW<ECS_EnemySpawner>>())
        {
            if (spawner.ValueRO.SpawnCount < spawner.ValueRO.MaxSpawnCount)
            {
                spawner.ValueRW.Timer += deltaTime;

                if (spawner.ValueRO.Timer >= spawner.ValueRO.SpawnInterval)
                {
                    spawner.ValueRW.Timer = 0f;

                    // Entity 생성 (ECB)
                    Entity enemy =
                        ecb.Instantiate(spawner.ValueRO.EnemyPrefab);

                    float3 spawnPos = waypoints[0].Position;

                    // 위치 설정
                    ecb.SetComponent(enemy,
                        LocalTransform.FromPosition(spawnPos));

                    // Path 연결 (중요)
                    ecb.AddComponent(enemy,
                        new ECS_PathReference
                        {
                            path = pathEntity
                        });

                    spawner.ValueRW.SpawnCount += 1;
                }
            }
        }

        // 마지막에 적용
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

}
