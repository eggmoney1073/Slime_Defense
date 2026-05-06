using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

public class SubSceneLoader : MonoBehaviour
{
    [SerializeField] string sceneGUID;

    void Start()
    {
        World world = World.DefaultGameObjectInjectionWorld;

        if (world == null)
        {
            Debug.LogError("World not initialized");
            return;
        }

        Unity.Entities.Hash128 guid = new Unity.Entities.Hash128(sceneGUID);

        SceneSystem.LoadSceneAsync(world.Unmanaged, guid);
    }
}