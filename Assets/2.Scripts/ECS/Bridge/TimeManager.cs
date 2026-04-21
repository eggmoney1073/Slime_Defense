using Unity.Entities;
using UnityEngine;

public class TimeManager : SingletonGameobject<TimeManager>
{
    private float _gameTimeScale = 1f;
    public void SetPause() => RequestTimeScaleChange(0f);
    public void SetTimeScaleNormal() => RequestTimeScaleChange(1f);
    public void SetTimeScaleFast() => RequestTimeScaleChange(2f);
    public void Resume() => RequestTimeScaleChange(_gameTimeScale);

    private void RequestTimeScaleChange(float targetTimeScale)
    {
        if (targetTimeScale > 0.3f)
        {
            _gameTimeScale = targetTimeScale;
        }

        World defaultWorld = World.DefaultGameObjectInjectionWorld;
        if (defaultWorld == null) return;

        EntityManager entityManager = defaultWorld.EntityManager;

        Entity requestEntity = entityManager.CreateEntity();
        entityManager.AddComponentData(requestEntity, new TimeScaleChangeRequest
        {
            targetTimeScale = targetTimeScale
        });
    }
}
