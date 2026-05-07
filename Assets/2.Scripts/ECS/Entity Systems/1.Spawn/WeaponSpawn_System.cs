using Unity.Burst;
using Unity.Entities;
using DefineEnums;
using System.Diagnostics;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(SpawnSystemGroup))]
partial struct WeaponSpawn_System : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (DynamicBuffer<WeaponPrefab> prefabBuffer in SystemAPI.Query<DynamicBuffer<WeaponPrefab>>())
        {
            if (prefabBuffer.Length == 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < prefabBuffer.Length; i++)
                {
                    Entity weaponEntity = prefabBuffer[i].weaponEntity;

                    Entity instantiatedWeapon = entityCommandBuffer.Instantiate(weaponEntity);
                    Entity entity = entityCommandBuffer.CreateEntity();
                    entityCommandBuffer.AddComponent(entity, new SpawnedWeaponEvent
                    {
                        type = (WeaponType)i,
                        weaponEntity = instantiatedWeapon
                    });
                }
            }

        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}
