using UnityEngine;

public static class ProjectileCount
{
    public static int Count { get; private set; }

    public static void SetCount(int enemyCount)
    {
        Count = enemyCount;
    }
}