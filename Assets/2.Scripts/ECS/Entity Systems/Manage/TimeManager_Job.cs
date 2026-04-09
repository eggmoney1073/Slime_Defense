using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
public partial struct TimeManager_Job : IJobEntity
{
    public RefRW<TimeScaleData> timeData;
    public EntityCommandBuffer.ParallelWriter ecbWriter;

    public void Execute(Entity entity, [EntityIndexInQuery] int index, in TimeScaleChangeRequest request)
    {
        float target = math.max(0f, request.targetTimeScale);

        timeData.ValueRW.timeScale = target;
        ecbWriter.DestroyEntity(index, entity);
    }
}
