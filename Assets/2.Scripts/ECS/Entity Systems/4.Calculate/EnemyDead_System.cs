using Unity.Burst;
using Unity.Entities;

[WithAll(typeof(EnemyTag))]
partial struct EnemyDead_System : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);

        RefRW<GameProgress> progress = SystemAPI.GetSingletonRW<GameProgress>();

        Entity soundBufferEntity = SystemAPI.GetSingletonEntity<SoundRequest>();

        foreach ((RefRO<EnemyHealth> enemayHealth, EnabledRefRW<LiveTag> liveTag, Entity entity)
                            in SystemAPI.Query<RefRO<EnemyHealth>, EnabledRefRW<LiveTag>>().WithEntityAccess())
        {
            if (enemayHealth.ValueRO.health <= 0)
            {
                liveTag.ValueRW = false;

                progress.ValueRW.kill += 1;

                ecb.AppendToBuffer(soundBufferEntity, new SoundRequest
                {
                    SFXType = SoundManager.SFXType.EnemyDeath
                });
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
