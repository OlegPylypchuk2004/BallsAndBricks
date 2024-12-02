using System;
using UnityEngine;

[Serializable]
public struct RowData
{
    public Vector2 Position;
    public BrickData[] BrickDatas;

    public RowData(Vector2 position, BrickData[] brickDatas)
    {
        Position = position;
        BrickDatas = brickDatas;
    }
}