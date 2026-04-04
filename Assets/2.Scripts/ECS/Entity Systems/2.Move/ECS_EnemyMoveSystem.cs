using Unity.Entities;
using Unity.Burst;

[BurstCompile]
[UpdateInGroup(typeof(MoveSystemGroup))]
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
    }
}
