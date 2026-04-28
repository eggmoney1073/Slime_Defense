using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[WithAll(typeof(EnemyTag))]
[WithAll(typeof(LiveTag))]
public partial struct EnemyDamaged_Job : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ecb;
    public Entity soundBufferEntity;

    public void Execute([EntityIndexInQuery] int entityInQueryIndex, ref DynamicBuffer<Damaged> damage, ref EnemyHealth health)
    {
        if (damage.Length == 0) return;

        for (int i = 0; i < damage.Length; i++)
        {
            health.health -= damage[i].damage;
        }

        ecb.AppendToBuffer(entityInQueryIndex, soundBufferEntity, new SoundRequest
        {
            SFXType = SoundManager.SFXType.EnemyHit
        });

        damage.Clear();
    }
}