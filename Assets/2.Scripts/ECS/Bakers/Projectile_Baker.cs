using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class Projectile_Baker : Baker<Projectile_Authoring>
{
    public override void Bake(Projectile_Authoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<ProjectileTag>(entity);

        AddComponent(entity, new LifetimeData
        {
            remainTime = authoring.Lifetime
        });

        AddComponent(entity, new ECS_MoveData
        {
            moveSpeed = authoring.Speed
        });

        AddComponent(entity, new Collision
        {
            radius = authoring.Radius
        });

        AddComponent<LiveTag>(entity);
    }
}
