using System;
using UnityEngine;

[Serializable]
public struct RowData
{
    public Vector2 Position;
    public BrickData[] BrickDatas;
    public PickupableBallData[] PickupableBallDatas;

    public RowData(Vector2 position, BrickData[] brickDatas, PickupableBallData[] pickupableBallDatas)
    {
        Position = position;
        BrickDatas = brickDatas;
        PickupableBallDatas = pickupableBallDatas;
    }
}