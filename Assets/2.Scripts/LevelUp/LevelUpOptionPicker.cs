using UnityEngine;

public static class LevelUpOptionPicker
{
    public static int[] Pick3(int optionCount)
    {
        if (optionCount < 3)
        {
            throw new System.ArgumentException("옵션 개수는 최소 3개 이상");
        }

        int[] pool = new int[optionCount];
        int[] result = new int[3];

        for (int i = 0; i < optionCount; i++)
        {
            pool[i] = i;
        }

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(i, optionCount);

            int temp = pool[i];
            pool[i] = pool[randomIndex];
            pool[randomIndex] = temp;

            result[i] = pool[i];
        }

        return result;
    }
}
