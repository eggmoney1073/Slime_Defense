using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

partial struct ECS_CollsitionSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float cellSize = 1.0f;
        int gridWidth = 100;
        int capacity = 1024;

        NativeParallelMultiHashMap<int, Entity> gridMap = new NativeParallelMultiHashMap<int, Entity>(capacity, Allocator.TempJob);

        GridBuildJob gridJob = new GridBuildJob
        {
            cellSize = cellSize,
            gridWidth = gridWidth,
            gridMap = gridMap.AsParallelWriter()
        };

        JobHandle gridHandle = gridJob.ScheduleParallel(state.Dependency);
        gridHandle.Complete();

        CollisionJob collisionJob = new CollisionJob
        {
            cellSize = cellSize,
            gridWidth = gridWidth,
            gridMap = gridMap,
            transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true),
            radiusLookup = SystemAPI.GetComponentLookup<CollisionRadius>(true)
        };

        JobHandle collisionHandle = collisionJob.ScheduleParallel(state.Dependency);
       
        collisionHandle.Complete();
        gridMap.Dispose();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
