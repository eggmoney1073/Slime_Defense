using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public struct SubSceneLoadRequest : IComponentData
{
    public EntitySceneReference SceneRef;
}

public class SubSceneTriggerAuthoring : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] SceneAsset _subSceneAsset;
#endif

    class Baker : Baker<SubSceneTriggerAuthoring>
    {
        public override void Bake(SubSceneTriggerAuthoring authoring)
        {
#if UNITY_EDITOR
            if (authoring._subSceneAsset == null)
            {
                return;
            }

            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SubSceneLoadRequest
            {
                SceneRef = new EntitySceneReference(authoring._subSceneAsset)
            });
#endif
        }
    }
}