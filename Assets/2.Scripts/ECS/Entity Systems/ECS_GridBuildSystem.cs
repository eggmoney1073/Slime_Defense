using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

partial struct ECS_GridBuildSystem : ISystem
{
    NativeParallelMultiHashMap<int, Entity> gridHashMap;

    public void OnCreate(ref SystemState state)
    {
        gridHashMap = new NativeParallelMultiHashMap<int, Entity>(1000 * 1000, Allocator.TempJob);
    }
    
    public void OnUpdate(ref SystemState state)
    {
        //gridHashMap.Clear();
        //int entityCount = SystemAPI.QueryBuilder().WithAll<CollisionRadius>().Build().CalculateEntityCount();        

        //GridBuildJob gridBuildJob = new GridBuildJob
        //{
        //    cellSize = 1f,
        //    gridWidth = 1000,
        //    gridMap = gridHashMap.AsParallelWriter()
        //};

        //state.Dependency = gridBuildJob.ScheduleParallel(state.Dependency);
        //state.Dependency.Complete();

        // 충돌 시스템과 공유하기 위한 처리
        //state.EntityManager.AddComponent(state.entity, new ECS_Grid
        //{
        //    grid = gridHashMap
        //});

    }

    public void OnDestroy()
    {
        gridHashMap.Dispose();
    }
}
