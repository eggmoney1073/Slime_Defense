using Unity.Entities;
using Unity.Burst;

[BurstCompile]
[UpdateInGroup(typeof(MoveSystemGroup))]
public partial struct EnemyMove_System :ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        EnemyMove_Job moveJob = new EnemyMove_Job
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            wayPointBufferLookup = SystemAPI.GetBufferLookup<ECS_WayPoint>(true)
        };

        moveJob.ScheduleParallel();
    }
}
