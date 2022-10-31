using Assets.Scripts.Levels;
using Assets.Scripts.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class GameConfiguration
    {
        public int MinLevelForRandom = 1;

        public float LevelsDataBaseVersion = 0;
        public float MinAppVersionForThisLevels = 0.1f;

        public float LocalizationsVersion = 0.1f;

        public List<LevelConfigData> Levels;
        public List<LocalizationKey> LocalizationKeys;

        public bool NeedUpdateLocalizations(float buildLocalizationsVersion) => buildLocalizationsVersion < LocalizationsVersion;
        public bool NeedUpdatedLevels(float buildLevelsDBVersion)
        {
            float appVersion = float.Parse(Application.version, CultureInfo.InvariantCulture); // System.Globalization.NumberStyles.AllowDecimalPoint);

            bool appVersionCorrect = appVersionCorrect = appVersion >= MinAppVersionForThisLevels;
            bool isBuildLevelsVersionLower = buildLevelsDBVersion < LevelsDataBaseVersion;

            bool needUpdate = appVersionCorrect && isBuildLevelsVersionLower;

            Debug.Log($"Need update levels = {needUpdate}. App version = {Application.version}, min app version for updating levels = {MinAppVersionForThisLevels}. Current db version = {buildLevelsDBVersion}, Server db versiom = {LevelsDataBaseVersion}");
            return needUpdate;
        }

#if UNITY_EDITOR
        public static GameConfiguration CreateActualConfiguration()
        {
            GameConfiguration config = new GameConfiguration();
            WriteLevelsDatabase(config);
            WriteLocalizationKeys(config);

            return config;
        }

        private static void WriteLevelsDatabase(GameConfiguration config)
        {
            var levelDataBases = Resources.FindObjectsOfTypeAll<LevelsDatabase>();

            if (levelDataBases == null || levelDataBases.Length == 0)
                Debug.Log("Dont't finded levels database!");
            else if (levelDataBases.Length > 1)
                Debug.Log("Finded more then one level databases!");
            else
            {
                LevelsDatabase database = levelDataBases.First();
                Debug.Log($"Level database getted. Version: {database.Version}");

                config.Levels = database.GetDefaultLevelsData();
                config.LevelsDataBaseVersion = database.Version;
            }
        }

        private static void WriteLocalizationKeys(GameConfiguration config)
        {
            var localizationDatabases = Resources.FindObjectsOfTypeAll<LocalizationDatabase>();

            if (localizationDatabases == null || localizationDatabases.Length == 0)
                Debug.Log("Dont't finded localizations database!");
            else if (localizationDatabases.Length > 1)
                Debug.Log("Finded more then one localizations databases!");
            else
            {
                LocalizationDatabase database = localizationDatabases.First();
                Debug.Log($"Localizations database getted. Version: {database.Version}");

                config.LocalizationKeys = database.GetKeysData().ToList();
                config.LocalizationsVersion = database.Version;
            }
        }
#endif
    }
}