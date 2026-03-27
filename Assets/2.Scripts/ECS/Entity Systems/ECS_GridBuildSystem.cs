using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

partial struct ECS_GridBuildSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        int entityCount = SystemAPI.QueryBuilder().WithAll<CollisionRadius>().Build().CalculateEntityCount();

        NativeParallelMultiHashMap<int,Entity> gridHashMap = new NativeParallelMultiHashMap<int, Entity>(entityCount, Allocator.TempJob);


    }
}
