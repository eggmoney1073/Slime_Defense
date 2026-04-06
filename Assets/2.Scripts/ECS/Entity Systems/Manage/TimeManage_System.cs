using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

partial struct TimeManager_System : ISystem
{
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        if(!SystemAPI.HasSingleton<TimeScaleData>())
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

        RefRW<TimeScaleData> timeData = SystemAPI.GetSingletonRW<TimeScaleData>();

        foreach (var (request, requestEntity)
                 in SystemAPI.Query<RefRO<TimeScaleChangeRequest>>().WithEntityAccess())
        {
            float target = math.max(0f, request.ValueRO.targetTimeScale);

            timeData.ValueRW.timeScale = target;
            state.EntityManager.DestroyEntity(requestEntity);
        }

        // 데이터 업데이트
        timeData.ValueRW.scaledDeltaTime = deltaTime * timeData.ValueRO.timeScale;
        timeData.ValueRW.unscaledDeltaTime = deltaTime;
        timeData.ValueRW.time += timeData.ValueRO.scaledDeltaTime;
        //TimeCount.AddTime(timeData.ValueRO.scaledDeltaTime);
    }
}
