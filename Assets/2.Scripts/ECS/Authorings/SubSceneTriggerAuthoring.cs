using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;

public struct SubSceneLoadRequest : IComponentData
{
    public EntitySceneReference SceneRef;
}

public class SubSceneTriggerAuthoring : MonoBehaviour
{
    [SerializeField] EntitySceneReference _sceneReference;

    class SubSceneTriggerBaker : Baker<SubSceneTriggerAuthoring>
    {
        public override void Bake(SubSceneTriggerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SubSceneLoadRequest
            {
                SceneRef = authoring._sceneReference
            });
        }
    }
}