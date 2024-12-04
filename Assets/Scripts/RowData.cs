using System;
using UnityEngine;

[Serializable]
public struct RowData
{
    public Vector2 Position;
    public BrickData[] BrickDatas;
    public PickupableBallData[] PickupableBallDatas;
    public PickupableCoinData[] PickupableCoinDatas;

    public RowData(Vector2 position, BrickData[] brickDatas, PickupableBallData[] pickupableBallDatas, PickupableCoinData[] pickupableCoinDatas)
    {
        Position = position;
        BrickDatas = brickDatas;
        PickupableBallDatas = pickupableBallDatas;
        PickupableCoinDatas = pickupableCoinDatas;
    }
}