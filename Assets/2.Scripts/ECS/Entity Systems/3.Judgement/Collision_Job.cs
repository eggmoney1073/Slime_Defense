using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[WithAll(typeof(ProjectileTag))]
public partial struct Collision_Job : IJobEntity
{
    public int _gridWidth;
    public float _cellSize;

    [NativeDisableParallelForRestriction]
    public BufferLookup<HitEnemyBufferElement> _hitBufferLookup;

    [ReadOnly]
    public NativeParallelMultiHashMap<int, Entity> _gridMap;

    // 적 컴포넌트 데이터 Lookup
    [ReadOnly]
    public ComponentLookup<LocalTransform> _transformLookup;
    [ReadOnly]
    public ComponentLookup<Collision> _radiusLookup;
    [NativeDisableParallelForRestriction]
    public BufferLookup<Damaged> _damagedBufferLookup;

    public void Execute(Entity projectileEntity, in LocalTransform transform, in Collision collider, in ProjectileDamage projectileDamage)
    {
        float3 position = transform.Position;

        int centerX = (int)math.floor(position.x / _cellSize) + (_gridWidth / 2);
        int centerY = (int)math.floor(position.z / _cellSize) + (_gridWidth / 2);

        int offsetY = -1;
        while (offsetY <= 1)
        {
            int offsetX = -1;
            while (offsetX <= 1)
            {
                int axisX = centerX + offsetX;
                int axisY = centerY + offsetY;
                int index = axisX + _gridWidth * axisY;

                NativeParallelMultiHashMapIterator<int> iterator;
                Entity other;

                bool found = _gridMap.TryGetFirstValue(index, out other, out iterator);

                if (found)
                {
                    do
                    {
                        if (_transformLookup.HasComponent(other) && _radiusLookup.HasComponent(other) && _damagedBufferLookup.HasBuffer(other))
                        {
                            LocalTransform otherLocalTransform = _transformLookup[other];
                            float3 otherPosition = otherLocalTransform.Position;

                            float otherRadius = _radiusLookup[other].radius;
                            float distanceSq = math.lengthsq(position - otherPosition);
                            float sumRadius = collider.radius + otherRadius;

                            DynamicBuffer<Damaged> damagedBuffer = _damagedBufferLookup[other];

                            if (distanceSq <= sumRadius * sumRadius)
                            {
                                DynamicBuffer<HitEnemyBufferElement> hitBuffer = _hitBufferLookup[projectileEntity];

                                bool alreadyHit = false;
                                int i = 0;

                                while (i < hitBuffer.Length)
                                {
                                    if (hitBuffer[i].enemy == other)
                                    {
                                        alreadyHit = true;
                                    }

                                    i++;
                                }

                                if (!alreadyHit)
                                {
                                    hitBuffer.Add(new HitEnemyBufferElement
                                    {
                                        enemy = other
                                    });

                                    // 적에게 대미지 기록
                                    damagedBuffer.Add(new Damaged
                                    {
                                        damage = projectileDamage.damage
                                    });
                                }

                            }
                        }
                    }
                    while (_gridMap.TryGetNextValue(out other, ref iterator));
                }

                offsetX++;
            }

            offsetY++;
        }
    }
}
