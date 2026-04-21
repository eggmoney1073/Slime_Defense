using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class LevelUpEcsBridge : MonoBehaviour
{
    [SerializeField] private LevelUpFlow _flow;

    private EntityManager _entityManager;
    private EntityQuery _entityQuery;

    private void Start()
    {
        World world = World.DefaultGameObjectInjectionWorld;
        _entityManager = world.EntityManager;
        _entityQuery = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<LevelUpEvent>());
    }

    private void Update()
    {
        using var events = _entityQuery.ToComponentDataArray<LevelUpEvent>(Allocator.Temp);
        using var entities = _entityQuery.ToEntityArray(Allocator.Temp);

        for (int i = 0; i < events.Length; i++)
        {
            _flow.EnqueueLevelUps(events[i].levelUpCount);
            _entityManager.DestroyEntity(entities[i]);
        }
    }
}