using System;
using UnityEngine;

public class Transformable
{
    public Transformable(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    public event Action Moved;
    public event Action<string> ExceptionCaught;

    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }

    public void MoveTo(Vector3 position)
    {
        Position = position;
        Moved?.Invoke();
    }

    public void WriteException(string exceptionMessage)
    {
        ExceptionCaught?.Invoke(exceptionMessage);
    }
}
