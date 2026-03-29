using Unity.Entities;
using UnityEngine;

class Grid_Baker : Baker<Grid_Authoring>
{
    public override void Bake(Grid_Authoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent<GridTag>(entity);
    }
}
