using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public int BestScore;
    public int CoinsCount;
    public bool IsSoundDisabled;
    public bool IsRateRewardClaimed;
    public int ChosenBallSkinIndex;
    public List<int> PurchasedBallSkinIndexes;

    public PlayerData()
    {
        PurchasedBallSkinIndexes = new List<int>
        {
            0
        };
    }
}