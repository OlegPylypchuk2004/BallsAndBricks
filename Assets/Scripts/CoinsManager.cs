using System;

public static class CoinsManager
{
    public static event Action<int> CountChanged;

    public static void Receive(int count)
    {
        PlayerData playerData = PlayerDataManager.LoadPlayerData();
        playerData.CoinsCount += count;

        PlayerDataManager.SavePlayerData(playerData);

        CountChanged?.Invoke(playerData.CoinsCount);
    }

    public static bool Spend(int count)
    {
        bool isSpended = false;

        PlayerData playerData = PlayerDataManager.LoadPlayerData();

        if (playerData.CoinsCount >= count)
        {
            playerData.CoinsCount -= count;
            isSpended = true;

            CountChanged?.Invoke(playerData.CoinsCount);
        }

        return isSpended;
    }
}