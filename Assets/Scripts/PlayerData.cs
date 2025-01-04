using System;

[Serializable]
public class PlayerData
{
    public int BestScore;
    public int CoinsCount;
    public bool IsSoundDisabled;
    public bool IsRateRewardClaimed;

    public PlayerData(int bestScore = 0, int coinsCount = 0)
    {
        BestScore = bestScore;
        CoinsCount = coinsCount;
    }
}