using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int BrickMovesCount;
    public int BrickDestroyCount;
    public int BallsCount;
    public int PickedCoinsCount;
    public float HorizontalBallsPosition;
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