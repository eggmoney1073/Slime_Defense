using Unity.Entities;
using UnityEngine;

public class ECS_Enemy_Baker : Baker<Enemy_Authoring>
{
    public override void Bake(Enemy_Authoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new EnemyTag());

        AddComponent(entity, new ECS_MoveData
        {
            Speed = authoring.Speed
        });

        AddComponent(entity, new ECS_WayPointFollower
        {
            CurrentIndex = 1
        });

        Debug.Log("Enemy Bake Complete");
    }
}
