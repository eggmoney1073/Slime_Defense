using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Scenes;
using UnityEngine;

public class SubSceneController : SingletonGameobject<SubSceneController>
{
    [SerializeField] EntitySceneReference _sceneReference;

    World _world;
    Entity _loadedSceneEntity = Entity.Null;

    void Start()
    {
        _world = World.DefaultGameObjectInjectionWorld;

        if (_world == null || !_world.IsCreated)
        {
            Debug.LogError("ECS World 없음");
            return;
        }

        SceneSystem.LoadParameters loadParameters = new SceneSystem.LoadParameters
        {
            Flags = SceneLoadFlags.LoadAdditive
        };

        _loadedSceneEntity = SceneSystem.LoadSceneAsync(_world.Unmanaged, _sceneReference, loadParameters);
    }
}