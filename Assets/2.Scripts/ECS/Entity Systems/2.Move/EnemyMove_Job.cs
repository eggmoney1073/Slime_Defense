using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[WithAll(typeof(EnemyTag))]
public partial struct EnemyMove_Job : IJobEntity
{
    public float deltaTime;

    [Unity.Collections.ReadOnly]
    public BufferLookup<ECS_WayPoint> wayPointBufferLookup;

    public void Execute(ref LocalTransform transform, ref EnemyWayPoint follower, 
                        in ECS_PathReference pathReference, in ECS_MoveData moveData, in EntityIndex indexRef)
    {
        if(!wayPointBufferLookup.HasBuffer(pathReference.path))
        {
            return;
        }

        DynamicBuffer<ECS_WayPoint> wayPoints = wayPointBufferLookup[pathReference.path];
        if (wayPoints.Length > 0)
        {
            int currentIndex = follower.currentIndex;

            float3 currentPosition = transform.Position;
            float3 destPosition = wayPoints[currentIndex].nodePosition;
            destPosition.z += indexRef.index * 0.001f;
            
            float speed = moveData.moveSpeed;

            float3 direction = destPosition - currentPosition;
            float distSq = math.lengthsq(direction);

            if (distSq > 0.00001f)
            {
                float3 normalizeDiretion = math.normalize(direction);

                transform.Position += normalizeDiretion * speed * deltaTime;
            }

            if (distSq < 0.1f * 0.1f)
            {
                int nextIndex = currentIndex + 1;

                if (nextIndex >= wayPoints.Length)
                {
                    nextIndex = 0;
                }

                follower.currentIndex = nextIndex;
            }
        }
    }
}
