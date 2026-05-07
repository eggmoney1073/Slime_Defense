using Unity.Burst;
using Unity.Entities;
using DefineEnums;

[BurstCompile]
[UpdateInGroup(typeof(SpawnSystemGroup))]
partial struct WeaponSpawn_System : ISystem
{
    bool _isSpawned;

    public void OnCreate(ref SystemState state)
    {
        _isSpawned = false;
        state.RequireForUpdate<WeaponPrefab>();
    }

    public void OnUpdate(ref SystemState state)
    {
        if (_isSpawned)
            return;

        EntityCommandBuffer entityCommandBuffer =
            new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (DynamicBuffer<WeaponPrefab> prefabBuffer in SystemAPI.Query<DynamicBuffer<WeaponPrefab>>())
        {
            if (prefabBuffer.Length == 0)
                continue;

            for (int i = 0; i < prefabBuffer.Length; i++)
            {
                Entity weaponEntity = prefabBuffer[i].weaponEntity;

                Entity instantiatedWeapon = entityCommandBuffer.Instantiate(weaponEntity);

                Entity eventEntity = entityCommandBuffer.CreateEntity();
                entityCommandBuffer.AddComponent(eventEntity, new SpawnedWeaponEvent
                {
                    type = (WeaponType)i,
                    weaponEntity = instantiatedWeapon
                });
            }

            _isSpawned = true;
            break;
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}