using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[WithAll(typeof(ProjectileTag))]
[WithAll(typeof(LiveTag))]
public partial struct ProjectilePierce_Job : IJobEntity
{
    public void Execute(Entity entity, ref PierceData pierce, EnabledRefRW<LiveTag> liveTag,
                        in DynamicBuffer<HitEnemyBufferElement> hitEnemyBuffer)
    {
        int hitCount = hitEnemyBuffer.Length;

        if (pierce.maxPierceCount <= hitCount)
        {
            liveTag.ValueRW = false;
        }
    }
}
