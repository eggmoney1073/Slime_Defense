using Unity.Entities;
using UnityEngine;

public class Enemy_Baker : Baker<Enemy_Authoring>
{
    public override void Bake(Enemy_Authoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<EnemyTag>(entity);
        AddComponent<LiveTag>(entity);

        AddComponent(entity, new ECS_MoveData
        {
            moveSpeed = authoring.Speed
        });

        AddComponent(entity, new EnemyWayPoint
        {
            currentIndex = 1
        });

        AddComponent(entity, new Collision
        {
            radius = authoring.Radius
        });

        //Debug.Log("Enemy Bake Complete");
    }
}
