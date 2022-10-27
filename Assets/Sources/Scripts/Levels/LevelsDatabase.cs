using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    [CreateAssetMenu]
    public class LevelsDatabase : ScriptableObject
    {
        [SerializeField] private UnitPropertiesDatabase _enemySolderLevelsDatabase;
        [SerializeField] private List<LevelConfig> _levelConfigs;

        private const int MinLevelForRandom = 1;
        public UnitPropertiesDatabase SolderLevelDatabase => _enemySolderLevelsDatabase;

        public LevelConfig GetLevelConfig(int levelId)
        {
            if(levelId >= _levelConfigs.Count)
                levelId = Random.Range(MinLevelForRandom, _levelConfigs.Count - 1);

            return _levelConfigs[levelId];
        }
    }
}
