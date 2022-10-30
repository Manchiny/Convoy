using Assets.Scripts.Levels;
using System;
using System.Collections.Generic;
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
        

    }
}