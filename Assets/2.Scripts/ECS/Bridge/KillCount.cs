using UnityEngine;

public static class KillCount
{
    public static int Kill { get; private set; }

    public static void SetKill(int kill)
    {
        Kill = kill;
    }
}
