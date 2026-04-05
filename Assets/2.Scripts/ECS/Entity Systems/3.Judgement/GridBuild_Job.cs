using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[WithAll(typeof(EnemyTag))]

public partial struct GridBuild_Job : IJobEntity
{
    public int gridWidth;
    public float cellSize;

    public NativeParallelMultiHashMap<int, Entity>.ParallelWriter gridMap;

    public void Execute(Entity entity, in LocalTransform transform)
    {
        float3 position = transform.Position;

        int axisX = (int)math.floor(position.x / cellSize) + (gridWidth / 2);
        int axisY = (int)math.floor(position.z / cellSize) + (gridWidth / 2);

        int index = axisX + gridWidth * axisY;

        gridMap.Add(index, entity);
    }
}

