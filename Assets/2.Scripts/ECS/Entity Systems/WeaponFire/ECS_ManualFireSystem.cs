using Unity.Entities;
using UnityEngine;

partial struct ECS_ManualFireSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach ((RefRW<FireRate> fireData, RefRW<ProjectilePrefab> projectile, Entity entity) in SystemAPI.Query<RefRW<FireRate>, RefRW<ProjectilePrefab>>().WithAll<WeaponTag>().WithEntityAccess())
        {
            //Debug.Log("Checking weapon fire...");

            if (!SystemAPI.IsComponentEnabled<WeaponEnabledTag>(entity))
                continue;

            float deltaTime = SystemAPI.Time.DeltaTime;            

            fireData.ValueRW.timer += deltaTime;
            if (fireData.ValueRO.timer >= fireData.ValueRO.coolDown)
            {
                fireData.ValueRW.timer = 0f;

                entityCommandBuffer.Instantiate(projectile.ValueRO.projectileEntity);

                Debug.Log("Fire!");
            }
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}
