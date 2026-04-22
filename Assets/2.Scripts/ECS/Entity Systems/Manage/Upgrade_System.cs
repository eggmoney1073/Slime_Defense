using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using DefineEnums;
using Unity.Mathematics;

[BurstCompile]
[UpdateInGroup(typeof(ManageSystemGroup))]
public partial struct Upgrade_System : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<UpgradeRequest>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        var fireRateLookup = SystemAPI.GetComponentLookup<FireRate>(isReadOnly: false);
        var damageLookup = SystemAPI.GetComponentLookup<WeaponDamage>(isReadOnly: false);
        var pierceLookup = SystemAPI.GetComponentLookup<WeaponPierce>(isReadOnly: false);
        var projLookup = SystemAPI.GetComponentLookup<WeaponProjectileCount>(isReadOnly: false);

        foreach ((RefRO<UpgradeRequest> request, Entity requestEntity)
                 in SystemAPI.Query<RefRO<UpgradeRequest>>().WithEntityAccess())
        {
            Entity weapon = request.ValueRO.weaponEntity;
            if (weapon == Entity.Null || !state.EntityManager.Exists(weapon))
            {
                ecb.DestroyEntity(requestEntity);
                continue;
            }

            switch (request.ValueRO.upgradeType)
            {
                case WeaponUpgradeType.FireRate:
                    if (fireRateLookup.HasComponent(weapon))
                    {
                        FireRate firerate = fireRateLookup[weapon];
                        firerate.coolDown = math.max(0.01f, firerate.coolDown * (1 - request.ValueRO.value / 100));
                        fireRateLookup[weapon] = firerate;
                    }
                    break;

                case WeaponUpgradeType.Damage:
                    if (damageLookup.HasComponent(weapon))
                    {
                        WeaponDamage damage = damageLookup[weapon];
                        damage.damage += request.ValueRO.value;
                        damageLookup[weapon] = damage;
                    }
                    break;

                case WeaponUpgradeType.Pierce:
                    if (pierceLookup.HasComponent(weapon))
                    {
                        WeaponPierce pierce = pierceLookup[weapon];
                        pierce.pierceCount += (int)request.ValueRO.value;
                        pierceLookup[weapon] = pierce;
                    }
                    break;

                case WeaponUpgradeType.ProjectileCount:
                    if (projLookup.HasComponent(weapon))
                    {
                        WeaponProjectileCount projectileCount = projLookup[weapon];
                        projectileCount.projectileCount += (int)request.ValueRO.value;
                        projLookup[weapon] = projectileCount;
                    }
                    break;
            }
            ecb.DestroyEntity(requestEntity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}