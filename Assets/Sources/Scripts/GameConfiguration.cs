using Assets.Scripts.Levels;
using System;
using System.Collections.Generic;
using System.IO;
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

        public bool NeedUpdateLocalizations(float buildLocalizationsVersion) => buildLocalizationsVersion < LocalizationsVersion; 
        public bool NeedUpdatedLevels(float buildLevelsDBVersion) => float.TryParse(Application.version, out float result) 
                                                                        && result >= MinAppVersionForThisLevels && buildLevelsDBVersion < LevelsDataBaseVersion;

#if UNITY_EDITOR
        public static GameConfiguration CreateActualConfiguration()
        {
            GameConfiguration config = new GameConfiguration();

            var levelDataBases = Resources.FindObjectsOfTypeAll<LevelsDatabase>();

            if (levelDataBases == null || levelDataBases.Length == 0)
                Debug.Log("Dont't finded levels database!");
            else if(levelDataBases.Length > 1)
                Debug.Log("Finded more then one level databases!");
            else
            {
                LevelsDatabase database = levelDataBases.First();
                Debug.Log($"Level database getted. Version: {database.Version}");

                config.Levels = database.GetDefaultLevelsData();
            }

            return config;
        }
#endif
    }
}