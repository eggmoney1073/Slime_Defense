using System.Collections.Generic;
using Unity.Collections;
using DefineEnums;
using Unity.Entities;
using UnityEngine;

public class WeaponDatabase : MonoBehaviour
{
    private EntityManager _entityManager;
    private EntityQuery _entityQuery;
    private Dictionary<WeaponType, Entity> _weaponEntityDict;

    private void Awake()
    {
        _weaponEntityDict = new Dictionary<WeaponType, Entity>();
        World world = World.DefaultGameObjectInjectionWorld;
        _entityManager = world.EntityManager;
        _entityQuery = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<SpawnedWeaponEvent>());
    }

    public Entity GetWeaponEntity(WeaponType type)
    {
        return _weaponEntityDict[type];
    }

    private void Update()
    {
        using var events = _entityQuery.ToComponentDataArray<SpawnedWeaponEvent>(Allocator.Temp);
        using var entities = _entityQuery.ToEntityArray(Allocator.Temp);

        for (int i = 0; i < events.Length; i++)
        {
            _weaponEntityDict.Add(events[i].type, events[i].weaponEntity);
            _entityManager.DestroyEntity(entities[i]);
        }
    }
}
