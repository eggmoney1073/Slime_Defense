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
                level = 0,
                levelUpKillCount = 15,
                levelUpPercent = 0,
                totalKill = 0,
            });
        }


        // // 시작하자마자 레벨업 이벤트 발생시키기
        // EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        // Entity levelUpEntity = ecb.CreateEntity();
        // ecb.AddComponent(levelUpEntity, new LevelUpEvent
        // {
        //     levelUpCount = 1,
        //     newLevel = 1
        // });
        // ecb.Playback(state.EntityManager);
        // ecb.Dispose();
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

            progress.ValueRW.levelUpKillCount += (progress.ValueRO.level - 1) * 5;

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

        // 게임 오버 체크

        EntityQuery gameOverEventQuery = SystemAPI.QueryBuilder().WithAll<GameOverEvent>().Build();
        if (!gameOverEventQuery.IsEmptyIgnoreFilter)
        {
            if (GameFlowManager.Instance != null && GameFlowManager.Instance.CurrentGameState != GameFlowManager.GameState.GameOver)
            {
                GameFlowManager.Instance.ChangeGameState(GameFlowManager.GameState.GameOver);
            }
            state.EntityManager.DestroyEntity(gameOverEventQuery);
        }

        // 게임 클리어 체크

        EntityQuery gameClearEventQuery = SystemAPI.QueryBuilder().WithAll<GameClearEvent>().Build();
        if (!gameClearEventQuery.IsEmptyIgnoreFilter)
        {
            if (GameFlowManager.Instance != null && GameFlowManager.Instance.CurrentGameState != GameFlowManager.GameState.Clear)
            {
                GameFlowManager.Instance.ChangeGameState(GameFlowManager.GameState.Clear);
            }
            state.EntityManager.DestroyEntity(gameClearEventQuery);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
