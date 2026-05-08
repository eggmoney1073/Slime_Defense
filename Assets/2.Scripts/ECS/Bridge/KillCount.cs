using UnityEngine;

public static class KillCount
{
    public static int Kill { get; private set; }

    public static void SetKill(int kill)
    {
        Kill = kill;
    }

    public static void AddKill(int kill)
    {
        Kill += kill;
    }

    public static void Reset()
    {
        Kill = 0;
    }
}
