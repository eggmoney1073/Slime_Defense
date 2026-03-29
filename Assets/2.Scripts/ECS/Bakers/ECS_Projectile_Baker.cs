using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class ECS_Projectile_Baker : Baker<Projectile_Authoring>
{
    public override void Bake(Projectile_Authoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new Direction
        {
            moveDirection = new float3(1, 0, 0),
        });

        AddComponent(entity, new DamageData
        {
            damage = authoring.Damage
        });

        AddComponent(entity, new LifetimeData
        {
            remainTime = authoring.Lifetime
        });

        AddComponent(entity, new ECS_MoveData
        {
            moveSpeed = authoring.Speed
        });

        AddComponent(entity, new CollisionRadius
        {
            radius = authoring.Radius
        });

        AddComponent<ProjectileTag>(entity);
    }
}
