using System;

[Serializable]
public class PlayerData
{
    public int BestScore;

    public PlayerData(int bestScore = 0)
    {
        BestScore = bestScore;
    }
}