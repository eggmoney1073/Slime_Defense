using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

public class SubSceneLoader : MonoBehaviour
{
    [SerializeField] string _sceneGUID = "73eda0c52b1dddd429f0e1933666822d";

    void Start()
    {
        World world = World.DefaultGameObjectInjectionWorld;

        if (world == null || string.IsNullOrEmpty(_sceneGUID))
        {
            return;
        }

        Unity.Entities.Hash128 guid = new Unity.Entities.Hash128(_sceneGUID);
        SceneSystem.LoadParameters loadParameters = new SceneSystem.LoadParameters();

        SceneSystem.LoadSceneAsync(world.Unmanaged, guid, loadParameters);
    }
}