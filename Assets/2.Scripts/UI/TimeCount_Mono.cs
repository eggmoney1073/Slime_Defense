using TMPro;
using UnityEngine;
using Unity.Entities;

public class TimeCount_Mono : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _countText;

    World _world;
    EntityManager _entityManager;
    EntityQuery _timeQuery;

    void Awake()
    {
        _world = World.DefaultGameObjectInjectionWorld;
        if (_world == null) return;

        _entityManager = _world.EntityManager;
        _timeQuery = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<TimeScaleData>());
    }

    void Update()
    {
        if (_world == null || !_world.IsCreated) return;
        if (_timeQuery.IsEmpty) return; // 싱글톤 아직 없을 수 있음

        TimeScaleData timeData = _timeQuery.GetSingleton<TimeScaleData>();

        double time = timeData.time;
        _countText.text = time.ToString();
    }
}
