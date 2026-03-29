using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[WithAll(typeof(CollisionRadius))]
public partial struct CollisionJob : IJobEntity
{
    public int gridWidth;
    public float cellSize;

    [ReadOnly]
    public NativeParallelMultiHashMap<int, Entity> gridMap;
    [ReadOnly]
    public ComponentLookup<LocalTransform> transformLookup;
    [ReadOnly]
    public ComponentLookup<CollisionRadius> radiusLookup;

    public void Execute(Entity entity, in LocalTransform transform, in CollisionRadius collider)
    {
        float3 position = transform.Position;

        int axisX = (int)math.floor(position.x / cellSize);
        int axisY = (int)math.floor(position.z / cellSize);

        int index = axisX + gridWidth * axisY;

        NativeParallelMultiHashMapIterator<int> iterator;
        Entity other;

        bool found = gridMap.TryGetFirstValue(index, out other, out iterator);

        if (found)
        {
            do
            {
                if (other != entity)
                {
                    LocalTransform otherLocalTransform = transformLookup[other];
                    float3 otherPosition = otherLocalTransform.Position;
                    float otherRadius = radiusLookup[other].radius;

                    if(math.distance(transform.Position, otherPosition) < collider.radius + otherRadius)
                    {

                        //Test
                        Debug.Log("Ãæµ¹");
                    }
                }
            }while(gridMap.TryGetNextValue(out other, ref iterator));
        }
    }
}
