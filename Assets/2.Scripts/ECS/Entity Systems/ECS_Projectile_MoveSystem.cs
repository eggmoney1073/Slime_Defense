using Unity.Burst;
using Unity.Entities;

partial struct ECS_Projectile_MoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EndSimulationEntityCommandBufferSystem.Singleton ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

        Projectile_Linear_MoveSystem projectileMoveJob = new Projectile_Linear_MoveSystem
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            ecbWriter = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
        };

        projectileMoveJob.ScheduleParallel();
    }
}
