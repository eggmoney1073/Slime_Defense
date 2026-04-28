using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(CalculateSystemGroup))]

partial struct EnemyDamaged_System : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        EnemyDamaged_Job enemyDamaged_Job = new EnemyDamaged_Job
        {
            ecb = ecb.AsParallelWriter(),
            soundBufferEntity = SystemAPI.GetSingletonEntity<SoundRequest>()
        };
        state.Dependency = enemyDamaged_Job.ScheduleParallel(state.Dependency);
        state.Dependency.Complete();

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
