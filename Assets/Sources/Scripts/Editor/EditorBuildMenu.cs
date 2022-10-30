using Assets.Scripts;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EditorBuildMenu
{
    private const string BuildDataFileName = "GameConfiguration.json";
    private static string BuildDataFolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),$"{Application.productName}Data", $"Data_{DateTime.Now.ToString("dd_MM_yyyy")}");

    [MenuItem("Build/Create Build Data")]
    public static void CreateBuildData()
    {
        GameConfiguration config = GameConfiguration.CreateActualConfiguration();

        if (CheckValid(config) == false)
            return;

        string dataString = JsonUtility.ToJson(config);

        if (Directory.Exists(BuildDataFolderPath) == false)
            Directory.CreateDirectory(BuildDataFolderPath);

        string fullPath = Path.Combine(BuildDataFolderPath, BuildDataFileName);

        File.CreateText(fullPath).Dispose();

        using (TextWriter writer = new StreamWriter(fullPath, false))
        {
            writer.WriteLine(dataString);
            writer.Close();
            Debug.Log($"Data created in path: {fullPath}");
        }
    }

    private static bool CheckValid(GameConfiguration config)
    {
        if(config == null || config.Levels == null || config.Levels.Count == 0)
        {
            Debug.LogError("Data is wrong!");
            return false;
        }

        return true;
    }
}
