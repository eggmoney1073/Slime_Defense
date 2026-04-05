using Unity.Burst;
using Unity.Entities;

partial struct GameProgress_System : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        if (!SystemAPI.HasSingleton<GameProgress>())
        {
            Entity entity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(entity, new GameProgress());
        }
    }

    public void OnUpdate(ref SystemState state)
    {
        RefRW<GameProgress> progress = SystemAPI.GetSingletonRW<GameProgress>();
        progress.ValueRW.time += SystemAPI.Time.DeltaTime;

        KillCount.SetKill(progress.ValueRO.kill);
        EXPCount.SetEXP(progress.ValueRO.exp);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
