using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(CalculateSystemGroup))]

partial struct EnemyDamaged_System : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EnemyDamaged_Job enemyDamaged_Job = new EnemyDamaged_Job();
        state.Dependency = enemyDamaged_Job.ScheduleParallel(state.Dependency);
        state.Dependency.Complete();
    }
}
