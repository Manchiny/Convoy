using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    [CreateAssetMenu]
    public class LevelsDatabase : ScriptableObject
    {
        [SerializeField] private float _dataBaseVersion = 0;
        [SerializeField] private UnitPropertiesDatabase _enemySolderLevelsDatabase;
        [SerializeField] private List<LevelConfig> _levelConfigs;

        private const string Tag = "[LevelDatatabse]";
        private List<LevelConfigData> _levels;

        public UnitPropertiesDatabase SolderLevelDatabase => _enemySolderLevelsDatabase;

        public void Init(List<LevelConfigData> levelData)
        {
            if (levelData == null || levelData.Count == 0)
                _levels = CreateDefaultLevelsData();
            else
                _levels = levelData;
        }

        public LevelConfigData GetLevelConfig(int levelId)
        {
            if(levelId >= _levels.Count)
                levelId = Random.Range(Game.Configuration.MinLevelForRandom, _levels.Count - 1);

            return _levels[levelId];
        }

        private List<LevelConfigData> CreateDefaultLevelsData()
        {
            List<LevelConfigData> levelConfigs = new();

            foreach (var level in _levelConfigs)
                levelConfigs.Add(level.GetData());

            Debug.Log($"{Tag}: levels loaded from build;");
            return levelConfigs;
        }
    }
}
