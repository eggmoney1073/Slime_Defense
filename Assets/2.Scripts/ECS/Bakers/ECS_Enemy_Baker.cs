using Unity.Entities;
using UnityEngine;

public class ECS_Enemy_Baker : Baker<Enemy_Authoring>
{
    public override void Bake(Enemy_Authoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<EnemyTag>(entity);
        AddComponent<EnemyDeadTag>(entity);
        SetComponentEnabled<EnemyDeadTag>(entity, false);

        AddComponent(entity, new ECS_MoveData
        {
            moveSpeed = authoring.Speed
        });

        AddComponent(entity, new ECS_WayPointFollower
        {
            currentIndex = 1
        });

        AddComponent(entity, new CollisionRadius
        {
            radius = authoring.Radius
        });

        //Debug.Log("Enemy Bake Complete");
    }
}
