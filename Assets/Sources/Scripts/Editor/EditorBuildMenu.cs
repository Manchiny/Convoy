using Assets.Scripts;
using Assets.Scripts.Levels;
using Assets.Scripts.Localization;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EditorBuildMenu
{
    private const string MainGameConfigFileName = "GameConfiguration.txt";
    private const string LocalizationsFileName = "LocalizationsData.txt";
    private const string LevelsFileName = "LevelsData.txt";
    private static string BuildDataFolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{Application.productName}Data", $"Data_{DateTime.Now.ToString("dd_MM_yyyy")}");

    [MenuItem("Build/Create Build Data")]
    public static void CreateBuildData()
    {
        string folderPath = BuildDataFolderPath;

        CreateMainConfigurationFile(folderPath);
        CreateLocalizationsDataFile(folderPath);
        CreateLevelesDataFile(folderPath);
    }

    #region Creating Build Data
    private static void CreateMainConfigurationFile(string folderPath)
    {
        GameConfiguration config = GameConfiguration.CreateActualConfiguration();
        string dataString = JsonUtility.ToJson(config);
        CreateFile(folderPath, MainGameConfigFileName, dataString);
    }

    private static void CreateLocalizationsDataFile(string folderPath)
    {
        LocalizationsData data = GameConfiguration.GetLocalizationKeysData();
        string dataString = JsonUtility.ToJson(data);
        CreateFile(folderPath, LocalizationsFileName, dataString);
    }

    private static void CreateLevelesDataFile(string folderPath)
    {
        LevelsDatabaseData data = GameConfiguration.GetLevelsDatabaseData();

        if (CheckValidLevels(data))
        {
            string dataString = JsonUtility.ToJson(data);
            CreateFile(folderPath, LevelsFileName, dataString);
        }
    }

    private static void CreateFile(string folderPath, string fileName, string data)
    {
        if (Directory.Exists(folderPath) == false)
            Directory.CreateDirectory(folderPath);

        string fullPath = Path.Combine(folderPath, fileName);

        File.CreateText(fullPath).Dispose();

        using (TextWriter writer = new StreamWriter(fullPath, false))
        {
            writer.WriteLine(data);
            writer.Close();
            Debug.Log($"Data created in path: {fullPath}");
        }
    }

    private static bool CheckValidLevels(LevelsDatabaseData data)
    {
        if (data == null || data.Levels == null || data.Levels.Count == 0)
        {
            Debug.LogError("Levels data is wrong!");
            return false;
        }

        return true;
    }

    private static void TryParseSaved(string fullPath)
    {
        GameConfiguration data = new GameConfiguration();

        if (File.Exists(fullPath))
        {
            data = JsonUtility.FromJson<GameConfiguration>(File.ReadAllText(fullPath));
            Debug.Log($"Parse config - LevelsVersion: {data.LevelsDataBaseVersion}, LocalVersion: {data.LocalizationsVersion}");
        }
        else
            Debug.Log("Parse false: File not founded;");
    }
    #endregion Creating Build Data
}
