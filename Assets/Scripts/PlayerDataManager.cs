using System.IO;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private static readonly string SavePath = $"{Application.persistentDataPath}/PlayerData.json";

    public static void SavePlayerData(PlayerData playerData)
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(SavePath, json);

        Debug.Log($"Player data saved to {SavePath}");
    }

    public static PlayerData LoadPlayerData()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("Save file not found. New game started");

            return new PlayerData();
        }

        string json = File.ReadAllText(SavePath);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);

        Debug.Log("Player data loaded successfully");

        return playerData;
    }

    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);

            Debug.Log("Player data file deleted");
        }
        else
        {
            Debug.Log("No player save file to delete");
        }
    }
}