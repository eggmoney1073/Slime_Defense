using Unity.Burst;
using Unity.Entities;

[WithAll(typeof(EnemyTag))]
partial struct EnemyDead_System : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRO<EnemyHealth> enemayHealth, EnabledRefRW<LiveTag> liveTag, Entity entity)
                            in SystemAPI.Query< RefRO<EnemyHealth>, EnabledRefRW<LiveTag>>().WithEntityAccess())
        {
            if (entity != null)
            {
                if(enemayHealth.ValueRO.health <= 0)
                {
                    liveTag.ValueRW = false;
                    RefRW<GameProgress> progress = SystemAPI.GetSingletonRW<GameProgress>();
                    progress.ValueRW.kill += 1;
                }
            }
        }
    }
}
