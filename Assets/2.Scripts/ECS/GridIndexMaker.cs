using UnityEngine;

public static class GridIndexMaker
{
    public static int GetGridIndex(Vector3 position, float cellSize, int gridWidth)
    {
        int axisX = (int)Mathf.Floor(position.x / cellSize) + (gridWidth / 2);
        int axisY = (int)Mathf.Floor(position.z / cellSize) + (gridWidth / 2);

        return axisX + gridWidth * axisY;
    }
}
