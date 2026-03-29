using Unity.Collections;
using Unity.Entities;

public struct ECS_Grid : IComponentData
{
    public NativeParallelMultiHashMap<int, Entity> grid;
}
