using System;
using UnityEngine;

public class Transformable
{
    public Transformable(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }

    public event Action Moved;

    public void MoveTo(Vector3 position)
    {
        Position = position;
        Moved?.Invoke();
    }
}
