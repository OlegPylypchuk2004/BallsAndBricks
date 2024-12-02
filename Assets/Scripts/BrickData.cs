using System;
using UnityEngine;

[Serializable]
public struct BrickData
{
    public int Number;
    public Vector2 Position;

    public BrickData(int number, Vector2 position)
    {
        Number = number;
        Position = position;
    }
}