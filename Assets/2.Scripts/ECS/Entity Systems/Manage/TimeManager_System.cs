using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
[UpdateInGroup(typeof(ManageSystemGroup))]
partial struct TimeManager_System : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        if (SystemAPI.HasSingleton<TimeScaleData>() == false)
        {
            Entity entity = state.EntityManager.CreateEntity();

            state.EntityManager.AddComponentData(entity, new TimeScaleData
            {
                timeScale = 1f,
                scaledDeltaTime = 0f,
                time = 0f,
                unscaledDeltaTime = 0f
            });
        }
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        float newTimeScale = SystemAPI.GetSingletonRW<TimeScaleData>().ValueRO.timeScale;

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach ((RefRO<TimeScaleChangeRequest> request, Entity entity) in SystemAPI.Query<RefRO<TimeScaleChangeRequest>>().WithEntityAccess())
        {
            newTimeScale = math.max(0f, request.ValueRO.targetTimeScale);
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();

        RefRW<TimeScaleData> timeData = SystemAPI.GetSingletonRW<TimeScaleData>();

        timeData.ValueRW.timeScale = newTimeScale;
        timeData.ValueRW.scaledDeltaTime = deltaTime * newTimeScale;
        timeData.ValueRW.unscaledDeltaTime = deltaTime;
        timeData.ValueRW.time += deltaTime * newTimeScale;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
}