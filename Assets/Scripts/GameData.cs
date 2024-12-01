using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
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