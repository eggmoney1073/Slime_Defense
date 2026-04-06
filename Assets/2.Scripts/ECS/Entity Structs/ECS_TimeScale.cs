using Unity.Entities;

public struct TimeScaleData : IComponentData
{
    public float timeScale;
    public float scaledDeltaTime;
    public float unscaledDeltaTime;
    public float time;
}

public struct TimeScaleChangeRequest : IComponentData
{
    public float targetTimeScale;
}
