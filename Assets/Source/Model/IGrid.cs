using UnityEngine;

public interface IGrid
{
    public IGridMember[,] Cells { get; }

    public int Size { get; }

    Vector3 CalculateWorldPosition(int[] gridPosition);

    int[] CalculateGridPosition(Vector3 worldPosition);

    void Place(PipeTemplate pipeTemplate);

    void Remove(PipeTemplate pipeTemplate);
}