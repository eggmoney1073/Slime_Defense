using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(SpawnSystemGroup))]
partial struct MainFire_System : ISystem
{
    EntityQuery _projectileQuery;
    int _spawnIndex;

    public void OnCreate(ref SystemState state)
    {
        _projectileQuery = state.GetEntityQuery(ComponentType.ReadOnly<ProjectileTag>());
        _spawnIndex = 0;
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        TimeScaleData timeData = SystemAPI.GetSingleton<TimeScaleData>();

        float deltaTime = timeData.scaledDeltaTime;

        if (deltaTime <= 0f) return;

        foreach ((RefRW<FireRate> fireData, RefRW<ProjectilePrefab> projectile, RefRO<WeaponDamage> weaponDamage, RefRO<WeaponPierce> pierce, RefRO<WeaponProjectileCount> projectileCount, Entity entity)
                                    in SystemAPI.Query<RefRW<FireRate>, RefRW<ProjectilePrefab>, RefRO<WeaponDamage>, RefRO<WeaponPierce>, RefRO<WeaponProjectileCount>>().WithAll<WeaponTag>().WithEntityAccess())
        {
            if (!SystemAPI.IsComponentEnabled<WeaponEnabledTag>(entity))
                continue;

            fireData.ValueRW.timer += deltaTime;

            if (fireData.ValueRO.timer >= fireData.ValueRO.coolDown)
            {
                fireData.ValueRW.timer -= fireData.ValueRO.coolDown;

                int shotCount = projectileCount.ValueRO.projectileCount;

                float3 aimDir = AimDirectionBridge.AimDirection;
                float baseAngle = math.atan2(aimDir.y, aimDir.x);
                float angleStep = math.radians(11f);

                float totalSpread = angleStep * (shotCount - 1);
                float startAngle = baseAngle - totalSpread / 2f;

                for (int i = 0; i < shotCount; i++)
                {
                    float currentAngle = startAngle + angleStep * i;
                    float3 currentDir = new float3(math.cos(currentAngle), math.sin(currentAngle), 0f);

                    Entity projectileEntity = ecb.Instantiate(projectile.ValueRO.projectileEntity);

                    float3 spawnPosition = new float3(0, 0, _spawnIndex * -0.00001f);
                    _spawnIndex++;

                    ecb.SetComponent(projectileEntity, LocalTransform.FromPositionRotationScale(spawnPosition, quaternion.RotateZ(currentAngle), 1f));

                    ecb.AddComponent(projectileEntity, new Direction
                    {
                        moveDirection = currentDir
                    });

                    ecb.AddComponent(projectileEntity, new ProjectileDamage
                    {
                        damage = weaponDamage.ValueRO.damage
                    });

                    ecb.AddComponent(projectileEntity, new PierceData
                    {
                        maxPierceCount = pierce.ValueRO.pierceCount
                    });

                    ecb.AddBuffer<HitEnemyBufferElement>(projectileEntity);
                }
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
        int allProjectileCount = _projectileQuery.CalculateEntityCount();
        ProjectileCount.SetCount(allProjectileCount);
    }
}
