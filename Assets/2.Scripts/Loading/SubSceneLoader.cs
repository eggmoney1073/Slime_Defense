using Unity.Entities;
using Unity.Scenes;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

public class SubSceneLoader : MonoBehaviour
{
    [SerializeField] string sceneGUID;
    [SerializeField] SubScene _subScene;

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

        //_subScene.SceneAsset = SceneSystem.GetSceneEntity(world.Unmanaged, guid);
        var query = world.EntityManager.CreateEntityQuery(typeof(LocalTransform));
        //Debug.Log("Entity count: " + query.CalculateEntityCount());
    }
}