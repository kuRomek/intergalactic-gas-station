using UnityEngine;

public interface IGrid
{
    public IGridMember[,] Cells { get; }

    Vector3 CalculateWorldPosition(int[] gridPosition);

    int[] CalculateGridPosition(Vector3 worldPosition);

    void Place(PipeTemplate pipeTemplate);

    void RemoveTemplate(PipeTemplate pipeTemplate);
}