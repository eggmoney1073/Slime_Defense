using Unity.Entities;
using Unity.Burst;

[BurstCompile]
[WithAll(typeof(ProjectileTag))]
[WithDisabled(typeof(ProjectileDeadTag))]
public partial struct Projectile_Lifetime_Job : IJobEntity
{
    public float _deltaTime;
    public EntityCommandBuffer.ParallelWriter _ecbWriter;

    public void Execute(Entity entity, ref LifetimeData lifetime, EnabledRefRW<ProjectileDeadTag> deadTag)
    {
        lifetime.remainTime -= _deltaTime;

        if(lifetime.remainTime < 0)
        {
            deadTag.ValueRW = true;
        }
    }
}
