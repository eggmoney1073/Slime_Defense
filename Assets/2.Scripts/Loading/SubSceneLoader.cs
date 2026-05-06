using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Scenes;
using UnityEngine;

public class SubSceneLoader : MonoBehaviour
{
    [SerializeField] EntitySceneReference _sceneReference;

    void Start()
    {
        World world = World.DefaultGameObjectInjectionWorld;

        if (world == null)
        {
            return;
        }

        SceneSystem.LoadParameters loadParameters = new SceneSystem.LoadParameters
        {
            Flags = SceneLoadFlags.LoadAdditive
        };

        SceneSystem.LoadSceneAsync(world.Unmanaged, _sceneReference, loadParameters);
    }
}