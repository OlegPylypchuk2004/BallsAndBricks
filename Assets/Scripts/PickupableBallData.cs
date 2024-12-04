using UnityEngine;
using System;

[Serializable]
public class PickupableBallData
{
    public Vector2 Position;

    public PickupableBallData(Vector2 position)
    {
        Position = position;
    }
}