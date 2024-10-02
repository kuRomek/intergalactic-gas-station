using System;
using UnityEngine;

public interface IGridMember
{
    int[] GridPosition { get; }

    void PlaceOnGrid(IGrid grid);
}
