using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
[UpdateInGroup(typeof(DestroySystemGroup))]
partial struct EntityDestroy_System : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);

        EntityDestroy_Job destroyJob = new EntityDestroy_Job
        {
            ecbWriter = entityCommandBuffer.AsParallelWriter()
        };

        state.Dependency = destroyJob.ScheduleParallel(state.Dependency);
        state.Dependency.Complete();

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}
