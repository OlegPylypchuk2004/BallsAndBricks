using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public List<BrickData> BrickDatas = new List<BrickData>();

    public void SaveBrick(int brickValue, Vector2 position)
    {
        BrickData brickData = new BrickData(brickValue, position);
        BrickDatas.Add(brickData);
    }
}