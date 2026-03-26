using Unity.Entities;
using UnityEngine;

class BaseWeapon_Baker : Baker<BaseWeapon_Authoring>
{
    public override void Bake(BaseWeapon_Authoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new FireRate
        {
            coolDown = authoring.Cooldown,
            timer = 0f
        });

        AddComponent(entity, new ManualAimTag { });
    }
}
