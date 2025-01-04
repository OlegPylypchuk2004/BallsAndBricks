using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int BrickMovesCount;
    public int BrickDestroyCount;
    public int BallsCount;
    public int PickedCoinsCount;
    public float HorizontalBallsPosition;
    public Vector2 LaunchDirection;
    public List<RowData> RowDatas;

    public GameData()
    {
        RowDatas = new List<RowData>();
    }

    public void SaveRow(RowData rowData)
    {
        RowDatas.Add(rowData);
    }
}