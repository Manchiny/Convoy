using Assets.Scripts.Levels;
using Assets.Scripts.Localization;
using System;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class GameConfiguration
    {
        public bool NeedShowInterstitialAfterLevelsComplete = false;
        public int LevelsCompletedCountForShowInterstitial = 2;

        public bool NeedShowInterstitialOnRestartLevel = true;
        public int LevelFailsCountForShowInterstitial = 3;

        public float LocalizationsVersion;

        public int MinLevelForRandom = 165;

        public float LevelsDataBaseVersion;
        public float MinAppVersionForThisLevels = 0.3f;


        [NonSerialized]
        public LocalizationsData LocalizationData;

        [NonSerialized]
        public LevelsDatabaseData LevelsDatabaseData;

        public bool NeedUpdateLocalizations(float buildLocalizationsVersion) => buildLocalizationsVersion < LocalizationsVersion;
        public bool NeedUpdatedLevels(float buildLevelsDBVersion)
        {
            float appVersion = float.Parse(Application.version, CultureInfo.InvariantCulture); // System.Globalization.NumberStyles.AllowDecimalPoint);

            bool appVersionCorrect = appVersionCorrect = appVersion >= MinAppVersionForThisLevels;
            bool isBuildLevelsVersionLower = buildLevelsDBVersion < LevelsDataBaseVersion;

            bool needUpdate = appVersionCorrect && isBuildLevelsVersionLower;

            Debug.Log($"Need update levels = {needUpdate}. App version = {Application.version}, min app version for updating levels = {MinAppVersionForThisLevels}. Current levels db version = {buildLevelsDBVersion}, Server levels db version = {LevelsDataBaseVersion}");
            return needUpdate;
        }

#if UNITY_EDITOR
        public static GameConfiguration CreateActualConfiguration()
        {
            GameConfiguration config = new GameConfiguration();

            config.LocalizationsVersion = GetCurrentLocalizationDatabase().Version;
            config.LevelsDataBaseVersion = GetCurrentLevelsDatabase().Version;

            return config;
        }

        public static LevelsDatabaseData GetLevelsDatabaseData()
        {
            LevelsDatabaseData data = null;
            LevelsDatabase database = GetCurrentLevelsDatabase();

            if(database != null)
            {
                data = new LevelsDatabaseData();
                data.Version = database.Version;           
                data.Levels = database.GetDefaultLevelsData();
            }

            return data;
        }

        public static LocalizationsData GetLocalizationKeysData()
        {
            LocalizationsData data = null;

            LocalizationDatabase database = GetCurrentLocalizationDatabase();
            
            if (database != null)
            {
                data = new LocalizationsData();
                data.Version = database.Version;
                data.LocalizationKeys = database.GetKeysData().ToList();
            }

            return data;
        }

        private static LocalizationDatabase GetCurrentLocalizationDatabase()
        {
            LocalizationDatabase database = null;

            var localizationDatabases = Resources.FindObjectsOfTypeAll<LocalizationDatabase>();

            if (localizationDatabases == null || localizationDatabases.Length == 0)
                Debug.Log("Dont't finded localizations database!");
            else if (localizationDatabases.Length > 1)
                Debug.Log("Finded more then one localizations databases!");
            else
            {
                database = localizationDatabases.First();
                Debug.Log($"Localizations database getted. Version: {database.Version}");
            }

            return database;
        }

        private static LevelsDatabase GetCurrentLevelsDatabase()
        {
            LevelsDatabase database = null;

            var levelDataBases = Resources.FindObjectsOfTypeAll<LevelsDatabase>();

            if (levelDataBases == null || levelDataBases.Length == 0)
                Debug.Log("Dont't finded levels database!");
            else if (levelDataBases.Length > 1)
                Debug.Log("Finded more then one level databases!");
            else
            {
                database = levelDataBases.First();
                Debug.Log($"Level database getted. Version: {database.Version}");
            }

            return database;
        }
#endif
    }
}