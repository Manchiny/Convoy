using System;
using System.Collections.Generic;

namespace Assets.Scripts.Levels
{
    [Serializable]
    public class LevelsDatabaseData
    {
        public float Version;
        public List<LevelConfigData> Levels;
    }
}
