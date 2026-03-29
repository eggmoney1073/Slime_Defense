using Unity.Entities;

partial struct ECS_ManualFireSystem : ISystem
{
    EntityQuery projectileQuery;

    public void OnCreate(ref SystemState state)
    {
        projectileQuery = state.GetEntityQuery(ComponentType.ReadOnly<ProjectileTag>());
    }

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
            }
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
        int projectileCount = projectileQuery.CalculateEntityCount();
        ProjectileCount.SetCount(projectileCount);
    }
}
