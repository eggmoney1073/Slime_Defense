using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

[UpdateInGroup(typeof(CalculateSystemGroup))]
partial struct ProjectileLiveCheck_System : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        ProjectileLifetime_Job lifetime_Job = new ProjectileLifetime_Job
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };

        JobHandle lifetimeJobHandle = lifetime_Job.ScheduleParallel(state.Dependency);
        lifetimeJobHandle.Complete();

        ProjectilePierce_Job pierce_Job = new ProjectilePierce_Job();

        state.Dependency = pierce_Job.ScheduleParallel(state.Dependency);
        state.Dependency.Complete();
    }
}
