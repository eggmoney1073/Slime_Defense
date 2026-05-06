using Unity.Entities;
using Unity.Scenes;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SubSceneLoadSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRO<SubSceneLoadRequest> request, Entity entity) in SystemAPI.Query<RefRO<SubSceneLoadRequest>>().WithEntityAccess())
        {
            SceneSystem.LoadParameters loadParameters = new SceneSystem.LoadParameters();

            SceneSystem.LoadSceneAsync(state.WorldUnmanaged, request.ValueRO.SceneRef, loadParameters);

            state.EntityManager.RemoveComponent<SubSceneLoadRequest>(entity);
        }
    }
}