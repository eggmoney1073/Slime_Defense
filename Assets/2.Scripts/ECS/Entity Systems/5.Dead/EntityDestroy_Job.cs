using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[WithDisabled(typeof(LiveTag))]
public partial struct EntityDestroy_Job : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ecbWriter;

    public void Execute(Entity entity, [EntityIndexInQuery] int index)
    {
        ecbWriter.DestroyEntity(index, entity);
    }
}
