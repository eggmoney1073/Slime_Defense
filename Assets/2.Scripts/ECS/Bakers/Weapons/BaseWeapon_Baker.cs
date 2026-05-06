using Unity.Entities;

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

        AddComponent(entity, new ECSWeaponType
        {
            type = authoring.weaponType
        });

        AddComponent<WeaponTag>(entity);
        AddComponent<WeaponEnabledTag>(entity);
        UnityEngine.Debug.Log("무기 베이커 실행됨");
    }
}
