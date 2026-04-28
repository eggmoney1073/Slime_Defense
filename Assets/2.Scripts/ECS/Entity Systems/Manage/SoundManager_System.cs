using Unity.Burst;
using Unity.Entities;

partial struct SoundManager_System : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        if (SystemAPI.TryGetSingletonEntity<SoundRequest>(out Entity soundEffectorEntity))
            return;

        Entity entity = state.EntityManager.CreateEntity();
        state.EntityManager.AddBuffer<SoundRequest>(entity);
    }
}
