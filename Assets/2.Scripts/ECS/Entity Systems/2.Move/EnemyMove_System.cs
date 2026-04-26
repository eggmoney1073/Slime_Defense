using Unity.Entities;
using Unity.Burst;

[BurstCompile]
[UpdateInGroup(typeof(MoveSystemGroup))]
public partial struct EnemyMove_System : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Persistent);
        EnemyMove_Job moveJob = new EnemyMove_Job
        {
            deltaTime = SystemAPI.GetSingleton<TimeScaleData>().scaledDeltaTime,
            wayPointBufferLookup = SystemAPI.GetBufferLookup<ECS_WayPoint>(true),
            ecbWriter = ecb.AsParallelWriter()
        };
        state.Dependency = moveJob.ScheduleParallel(state.Dependency);
        state.Dependency.Complete();

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
