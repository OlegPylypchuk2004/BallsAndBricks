using System;
using UnityEngine;

[Serializable]
public class PickupableCoinData
{
    public Vector2 Position;

    public PickupableCoinData(Vector2 position)
    {
        Position = position;
    }
}