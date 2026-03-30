using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using static UnityEditor.Experimental.GraphView.Port;

partial struct ECS_CollisionSystem : ISystem
{
    NativeParallelMultiHashMap<int, Entity> _gridMap;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        int capacity = 1024;

        _gridMap = new NativeParallelMultiHashMap<int, Entity>(capacity, Allocator.TempJob);
    }

    //[BurstCompile]
    //public void OnUpdate(ref SystemState state)
    //{
    //    float cellSize = 1.0f;
    //    int gridWidth = 100;

    //    GridBuildJob gridJob = new GridBuildJob
    //    {
    //        cellSize = cellSize,
    //        gridWidth = gridWidth,
    //        gridMap = _gridMap.AsParallelWriter()
    //    };

    //    JobHandle gridHandle = gridJob.ScheduleParallel(state.Dependency);
    //    gridHandle.Complete();

    //    CollisionJob collisionJob = new CollisionJob
    //    {
    //        cellSize = cellSize,
    //        gridWidth = gridWidth,
    //        gridMap = _gridMap,
    //        transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true),
    //        radiusLookup = SystemAPI.GetComponentLookup<CollisionRadius>(true)
    //    };

    //    JobHandle collisionHandle = collisionJob.ScheduleParallel(state.Dependency);
       
    //    collisionHandle.Complete();
    //    _gridMap.Clear();
    //}

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        _gridMap.Dispose();
    }
}
