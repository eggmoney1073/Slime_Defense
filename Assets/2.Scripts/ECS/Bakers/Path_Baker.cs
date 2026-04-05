using Unity.Entities;
using UnityEngine;

public class Path_Baker : Baker<Enemy_PathMaker>
{
    public override void Bake(Enemy_PathMaker authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new PathTag());

        DynamicBuffer<ECS_WayPoint> buffer = AddBuffer<ECS_WayPoint>(entity);

        Transform[] path = authoring.GetPathArray();

        if (path != null)
        {
            int i = 0;
            while (i < path.Length)
            {
                buffer.Add(new ECS_WayPoint
                {
                    nodePosition = path[i].position
                });

                i++;
            }
        }

        //Debug.Log("ECS_PathBake Complete");
    }
}
