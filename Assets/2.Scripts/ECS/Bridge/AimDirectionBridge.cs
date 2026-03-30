using Unity.Mathematics;
using UnityEngine;

public static class AimDirectionBridge
{
    public static float3 AimDirection { get; private set; }

    /// <summary>
    /// 기본 무기 발사 방향을 지정하는 함수.
    /// (평면이라 y축은 0으로 고정)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public static void SetAimDirection(float x, float z)
    {
        AimDirection = new float3(x,z,0);
        //Debug.Log(AimDirection);
    }
}
