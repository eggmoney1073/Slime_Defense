using UnityEngine;
using TMPro;
using Unity.Entities;


public class TotalTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalTimeText;
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
    private void OnEnable()
    {
        if (_world == null || !_world.IsCreated) return;
        if (_timeQuery.IsEmpty) return;

        TimeScaleData timeData = _timeQuery.GetSingleton<TimeScaleData>();

        float time = timeData.time;
        _totalTimeText.text = TimeCalculater.Calculate_MM_SS(time);
    }
}
