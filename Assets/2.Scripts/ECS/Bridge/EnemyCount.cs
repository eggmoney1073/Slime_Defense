using UnityEngine;

public static class EnemyCount
{
    public static int Count { get; private set; }

    public static void SetCount(int enemyCount)
    {
        Count = enemyCount;
    }
}
