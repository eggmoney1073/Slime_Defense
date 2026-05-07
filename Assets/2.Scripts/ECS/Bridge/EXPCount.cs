using UnityEngine;

public static class EXPCount
{
    public static float Exp { get; private set; }

    public static void SetEXP(float exp)
    {
        Exp = exp;
    }

    public static void Reset()
    {
        Exp = 0;
    }
}
