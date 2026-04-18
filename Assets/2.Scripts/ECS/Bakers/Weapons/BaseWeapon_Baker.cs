using Unity.Entities;
using Unity.VisualScripting;
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

        AddComponent(entity, new WeaponDamage
        {
            damage = authoring.damage
        });

        AddComponent(entity, new WeaponPierce
        {
            pierceCount = authoring.pierceCount
        });

        Entity projectile = GetEntity(authoring.projectilePrefab, TransformUsageFlags.Dynamic);

        AddComponent(entity, new ProjectilePrefab
        {
            projectileEntity = projectile
        });

        AddComponent(entity, new WeaponProjectileCount
        {
            projectileCount = authoring.projectileCount
        });


        AddComponent<WeaponTag>(entity);
        AddComponent<ManualAimTag>(entity);
        AddComponent<WeaponEnabledTag>(entity);
    }
}
