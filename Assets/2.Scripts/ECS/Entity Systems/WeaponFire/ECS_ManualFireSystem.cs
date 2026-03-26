using Unity.Entities;
using UnityEngine;

partial struct ECS_ManualFireSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<FireRate> fireData, Entity entity) in SystemAPI.Query<RefRW<FireRate>>().WithAll<WeaponTag>().WithEntityAccess())
        {
            Debug.Log("Checking weapon fire...");

            if (!SystemAPI.IsComponentEnabled<WeaponEnabledTag>(entity))
                continue;

            float deltaTime = SystemAPI.Time.DeltaTime;

            fireData.ValueRW.timer += deltaTime;
            if (fireData.ValueRO.timer >= fireData.ValueRO.coolDown)
            {
                fireData.ValueRW.timer = 0f;
                Debug.Log("Fire!");
            }
        }
    }
}
