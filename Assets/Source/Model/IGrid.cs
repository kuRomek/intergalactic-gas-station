using UnityEngine;

public interface IGrid
{
    const int Size = 5;

    Vector3 CalculateWorldPosition(int[] gridPosition);

    int[] CalculateGridPosition(Vector3 worldPosition);
}