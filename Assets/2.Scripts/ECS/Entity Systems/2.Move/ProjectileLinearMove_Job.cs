using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[WithAll(typeof(ProjectileTag))]
public partial struct ProjectileLinearMove_Job : IJobEntity
{
    public float _deltaTime;

    public void Execute(Entity entity, ref LocalTransform transform,
                        in Direction direction, in ECS_MoveData moveData)
    {
        transform.Position += direction.moveDirection * moveData.moveSpeed * _deltaTime;
    }
}
