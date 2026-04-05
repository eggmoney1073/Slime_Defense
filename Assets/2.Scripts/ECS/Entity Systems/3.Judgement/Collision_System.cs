using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(JudgementSystemGroup))]
partial struct Collision_System : ISystem
{
    NativeParallelMultiHashMap<int, Entity> _gridMap;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        int capacity = 1024;

        _gridMap = new NativeParallelMultiHashMap<int, Entity>(capacity, Allocator.Persistent);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _gridMap.Clear();

        float cellSize = 1.0f;
        int gridWidth = 100;

        GridBuild_Job gridJob = new GridBuild_Job
        {
            cellSize = cellSize,
            gridWidth = gridWidth,
            gridMap = _gridMap.AsParallelWriter()
        };

        JobHandle gridHandle = gridJob.ScheduleParallel(state.Dependency);
        gridHandle.Complete();

        Collision_Job collisionJob = new Collision_Job
        {
            _cellSize = cellSize,
            _gridWidth = gridWidth,
            _gridMap = _gridMap,
            _hitBufferLookup = SystemAPI.GetBufferLookup<HitEnemyBufferElement>(false),
            _transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true),
            _radiusLookup = SystemAPI.GetComponentLookup<Collision>(true),
            _damagedBufferLookup = SystemAPI.GetBufferLookup<Damaged>(false)
        };

        JobHandle collisionHandle = collisionJob.ScheduleParallel(state.Dependency);
        state.Dependency = collisionHandle;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        _gridMap.Dispose();
    }
}
