using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
//using UnityEngine;

[BurstCompile]
[WithAll(typeof(ProjectileTag))]
public partial struct Projectile_Linear_MoveSystem : IJobEntity
{
    public float deltaTime;
    public EntityCommandBuffer.ParallelWriter ecbWriter;

    public void Execute(Entity entity, ref LocalTransform transform, ref LifetimeData lifetime, [EntityIndexInQuery] int index,
                        in Direction direction, in ECS_MoveData moveData)
    {
        transform.Position += direction.moveDirection * moveData.moveSpeed * deltaTime;

        lifetime.remainTime -= deltaTime;

        if (lifetime.remainTime <= 0)
        {
            ecbWriter.DestroyEntity(index, entity);
        }
    }
}
