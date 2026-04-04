using Unity.Entities;
using UnityEngine;

class BaseWeapon_Baker : Baker<BaseWeapon_Authoring>
{
    public override void Bake(BaseWeapon_Authoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new FireRate
        {
            coolDown = authoring.cooldown,
            timer = 0f
        });

        Entity projectile = GetEntity(authoring.projectilePrefab, TransformUsageFlags.Dynamic);

        AddComponent(entity, new ProjectilePrefab
        {
            projectileEntity = projectile
        });

        AddComponent<WeaponTag>(entity);
        AddComponent<ManualAimTag>(entity);

        AddComponent<WeaponEnabledTag>(entity);
        SetComponentEnabled<WeaponEnabledTag>(entity, true);

        //Debug.Log("기본 무기 베이커 완료");
    }
}
