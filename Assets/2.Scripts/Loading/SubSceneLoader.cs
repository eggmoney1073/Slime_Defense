using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

public class SubSceneLoader : MonoBehaviour
{
    [SerializeField] SubScene subScene;

    void Start()
    {
        var world = World.DefaultGameObjectInjectionWorld;

        if (world == null)
        {
            Debug.LogError("World not initialized");
            return;
        }

        var sceneGUID = subScene.SceneGUID;

        SceneSystem.LoadSceneAsync(world.Unmanaged, sceneGUID);
    }
}