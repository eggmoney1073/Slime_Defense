using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct ECS_EnemyMoveSystem :ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        EnemyMoveJob moveJob = new EnemyMoveJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            wayPointBufferLookup = SystemAPI.GetBufferLookup<ECS_WayPoint>(true)
        };

        moveJob.ScheduleParallel();

        //float deltaTime = SystemAPI.Time.DeltaTime;
        //foreach (
        //   (RefRW<LocalTransform> transform,RefRW<ECS_WayPointFollower> follower,RefRO<ECS_PathReference> pathRef,RefRO<ECS_MoveData> moveData)
        //    in SystemAPI.Query<RefRW<LocalTransform>, RefRW<ECS_WayPointFollower>, RefRO<ECS_PathReference>, RefRO<ECS_MoveData>>().WithAll<EnemyTag>())
        //{
        //    DynamicBuffer<ECS_WayPoint> wayPoints = SystemAPI.GetBuffer<ECS_WayPoint>(pathRef.ValueRO.path);
        //    if (wayPoints.Length > 0)
        //    {
        //        int index = follower.ValueRO.currentIndex;

        //        float3 currentPosition = transform.ValueRO.Position;
        //        float3 destPosition = wayPoints[index].nodePosition;
        //        float speed = moveData.ValueRO.moveSpeed;

        //        float3 direction = destPosition - currentPosition;
        //        float distSq = math.lengthsq(direction);

        //        if (distSq > 0.00001f)
        //        {
        //            float3 normalizeDiretion = math.normalize(direction);

        //            transform.ValueRW.Position += normalizeDiretion * speed * deltaTime;
        //        }

        //        if (distSq < 0.1f * 0.1f)
        //        {
        //            int nextIndex = index + 1;

        //            if (nextIndex >= wayPoints.Length)
        //            {
        //                nextIndex = 0;
        //            }

        //            follower.ValueRW.currentIndex = nextIndex;
        //        }
        //    }
        //}
    }
}
