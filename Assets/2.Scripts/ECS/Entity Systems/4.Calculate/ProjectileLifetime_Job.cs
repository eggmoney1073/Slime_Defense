using Unity.Entities;
using Unity.Burst;

[BurstCompile]
[WithAll(typeof(ProjectileTag))]
[WithAll(typeof(LiveTag))]
public partial struct ProjectileLifetime_Job : IJobEntity
{
    public float deltaTime;

    public void Execute(Entity entity, ref LifetimeData lifetime, EnabledRefRW<LiveTag> liveTag)
    {
        lifetime.remainTime -= deltaTime;

        if(lifetime.remainTime < 0)
        {
            liveTag.ValueRW = false;
        }
    }
}
