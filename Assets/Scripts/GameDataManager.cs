using System.IO;
using UnityEngine;

public static class GameDataManager
{
    private static readonly string SavePath = $"{Application.persistentDataPath}/GameData.json";

    public static void SaveGameData(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(SavePath, json);

        Debug.Log($"Game data saved to {SavePath}");
    }

    public static GameData LoadGameData()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("Save file not found. New game started");

            return new GameData();
        }

        string json = File.ReadAllText(SavePath);
        GameData gameData = JsonUtility.FromJson<GameData>(json);

        Debug.Log("Game data loaded successfully");

        return gameData;
    }

    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);

            Debug.Log("Save file deleted");
        }
        else
        {
            Debug.Log("No save file to delete");
        }
    }
}