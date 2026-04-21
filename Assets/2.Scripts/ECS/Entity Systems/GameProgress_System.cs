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
            state.EntityManager.AddComponentData(entity, new GameProgress
            {
                kill = 0,
                time = 0,
                level = 1,
                levelUpKillCount = 25,
                levelUpPercent = 0
            });
        }
    }

    public void OnUpdate(ref SystemState state)
    {
        RefRW<GameProgress> progress = SystemAPI.GetSingletonRW<GameProgress>();

        // 시간 계산
        progress.ValueRW.time += SystemAPI.Time.DeltaTime;

        // 킬 계산
        int kills = progress.ValueRO.kill;
        KillCount.SetKill(kills);

        // 레벨업 계산
        int levelUpsThisFrame = 0;

        while (kills >= progress.ValueRO.levelUpKillCount)
        {
            kills -= progress.ValueRO.levelUpKillCount;

            int currentLevel = progress.ValueRO.level;
            progress.ValueRW.level = currentLevel + 1;

            progress.ValueRW.levelUpKillCount = 25 + (progress.ValueRO.level - 1) * 5;

            levelUpsThisFrame++;
        }

        progress.ValueRW.kill = kills;

        float percent = (float)kills / progress.ValueRO.levelUpKillCount;
        progress.ValueRW.levelUpPercent = Unity.Mathematics.math.saturate(percent);
        EXPCount.SetEXP(percent);

        // 레벨업 이벤트 생성
        if (levelUpsThisFrame > 0)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            Entity entity = ecb.CreateEntity();
            ecb.AddComponent(entity, new LevelUpEvent
            {
                levelUpCount = levelUpsThisFrame,
                newLevel = progress.ValueRO.level
            });
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
