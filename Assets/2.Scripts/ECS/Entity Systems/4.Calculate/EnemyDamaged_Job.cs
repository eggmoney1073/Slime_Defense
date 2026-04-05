using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[WithAll(typeof(EnemyTag))]
public partial struct EnemyDamaged_Job : IJobEntity
{
    public void Execute(Entity enemy, ref DynamicBuffer<Damaged> damage, ref EnemyHealth health, EnabledRefRW<LiveTag> liveTag)
    {
        for (int i = 0; i < damage.Length; i++)
        {
            health.health -= damage[i].damage;
        }

        damage.Clear();
    }
}
