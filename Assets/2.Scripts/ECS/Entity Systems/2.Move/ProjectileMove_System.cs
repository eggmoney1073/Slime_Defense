using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[UpdateInGroup(typeof(MoveSystemGroup))]
partial struct ProjectileMove_System : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EndSimulationEntityCommandBufferSystem.Singleton ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

        ProjectileLinearMove_Job projectileMoveJob = new ProjectileLinearMove_Job
        {
            _deltaTime = SystemAPI.Time.DeltaTime
        };

        projectileMoveJob.ScheduleParallel();
    }
}
