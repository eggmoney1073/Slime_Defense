using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(SpawnSystemGroup))]
partial struct ManualFire_System : ISystem
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

        foreach ((RefRW<FireRate> fireData, RefRW<ProjectilePrefab> projectile, RefRO<WeaponDamage> weaponDamage, RefRO<WeaponPierce> pierce,Entity entity) 
                                    in SystemAPI.Query<RefRW<FireRate>, RefRW<ProjectilePrefab>, RefRO<WeaponDamage>, RefRO<WeaponPierce>> ().WithAll<WeaponTag>().WithEntityAccess())
        {
            //Debug.Log("Checking weapon fire...");

            if (!SystemAPI.IsComponentEnabled<WeaponEnabledTag>(entity))
                continue;

            float deltaTime = SystemAPI.Time.DeltaTime;

            fireData.ValueRW.timer += deltaTime;
            if (fireData.ValueRO.timer >= fireData.ValueRO.coolDown)
            {
                fireData.ValueRW.timer = 0f;

                Entity projectileEntity = ecb.Instantiate(projectile.ValueRO.projectileEntity);

                float3 dir = AimDirectionBridge.AimDirection;
                float angle = math.atan2(dir.y, dir.x);
                float3 spawnPosition = new float3(0, 0, _spawnIndex * -0.001f);
                _spawnIndex++;

                ecb.SetComponent(projectileEntity, LocalTransform.FromPositionRotationScale(spawnPosition, quaternion.RotateZ(angle), 1f));

                ecb.AddComponent(projectileEntity, new Direction
                {
                    moveDirection = dir
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

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
        int projectileCount = _projectileQuery.CalculateEntityCount();
        ProjectileCount.SetCount(projectileCount);
    }
}
