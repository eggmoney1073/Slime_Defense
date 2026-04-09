using Unity.Entities;
using UnityEngine;

public class TimeManager : SingletonGameobject<TimeManager>
{
    public void SetPause() => RequestTimeScaleChange(0f);
    public void SetTimeScaleNormal() => RequestTimeScaleChange(1f);
    public void SetTimeScaleFast() => RequestTimeScaleChange(2f);

    void RequestTimeScaleChange(float targetTimeScale)
    {
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
