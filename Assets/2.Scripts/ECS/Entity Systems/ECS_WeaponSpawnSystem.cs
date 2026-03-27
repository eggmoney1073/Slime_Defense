using UnityEngine;
using Unity.Entities;

partial struct ECS_WeaponSpawnSystem : ISystem
{
    bool _isInitialized;

    public void OnCreate(ref SystemState state)
    {
        _isInitialized = false;
    }

    public void OnUpdate(ref SystemState state)
    {
        if (_isInitialized)
        {
            return;
        }

        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (DynamicBuffer<WeaponPrefab> prefabBuffer in SystemAPI.Query<DynamicBuffer<WeaponPrefab>>())
        {
            for (int i = 0; i < prefabBuffer.Length; i++)
            {
                Entity weaponEntity = prefabBuffer[i].weaponEntity;

                entityCommandBuffer.Instantiate(weaponEntity);
                //Debug.Log("Instantiate");
            }
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();

        _isInitialized = true;
    }
}
