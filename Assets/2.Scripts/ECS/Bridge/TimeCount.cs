using UnityEngine;

public class TimeCount
{
    public static float Time { get; private set; }

    public static void SetTime(float time)
    {
        Time = time;
    }

    public static void ResetTime()
    {
        Time = 0f;
    }

    public static void AddTime(float time)
    {
        Time += time;
    }
}
