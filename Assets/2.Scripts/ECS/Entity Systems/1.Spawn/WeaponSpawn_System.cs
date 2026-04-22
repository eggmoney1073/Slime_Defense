using Unity.Burst;
using Unity.Entities;
using DefineEnums;

[BurstCompile]
[UpdateInGroup(typeof(SpawnSystemGroup))]
partial struct WeaponSpawn_System : ISystem
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

                Entity instantiatedWeapon = entityCommandBuffer.Instantiate(weaponEntity);

                // 무기 생성 이벤트
                Entity entity = entityCommandBuffer.CreateEntity();
                entityCommandBuffer.AddComponent(entity, new SpawnedWeaponEvent
                {
                    type = (WeaponType)i,
                    weaponEntity = instantiatedWeapon
                });
            }
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();

        _isInitialized = true;
    }
}
