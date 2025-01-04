using System;

public static class CoinsManager
{
    public static event Action<int> CountChanged;

    public static bool Spend(int count)
    {
        bool isSpended = false;

        PlayerData playerData = PlayerDataManager.LoadPlayerData();

        if (playerData.CoinsCount >= count)
        {
            playerData.CoinsCount -= count;
            isSpended = true;

            PlayerDataManager.SavePlayerData(playerData);

            CountChanged?.Invoke(playerData.CoinsCount);
        }

        return isSpended;
    }
}