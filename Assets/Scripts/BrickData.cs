using System;
using UnityEngine;

[Serializable]
public struct BrickData
{
    public int Number;
    public Vector2 Position;

    public BrickData(int brickValue, Vector2 position)
    {
        Number = brickValue;
        Position = position;
    }
}