using System;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Units.Enemy;

namespace Assets.Scripts.Levels
{
    [CreateAssetMenu]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] public int _roadPartCount;
        [Range(2, 9)]
        [SerializeField] private int _maxMovableEnemyiesInGroup = 6;
        [Range(2, 9)]
        [SerializeField] private int _minMovableEnemyiesInGroup = 3;
        [Space]
        [Range(1, 7)]
        [SerializeField] private int _maxOnShelterEnemiesInGroup = 4;
        [Range(1, 7)]
        [SerializeField] private int _minOnShelterEnemiesInGroup = 1;
        [Space]
        [Range(0, 4)]
        [SerializeField] private int _minSolderLevel = 0;
        [Range(0, 4)]
        [SerializeField] private int _maxSolderLevel = 0;
        [Space]
        [Range(0, 1)]
        [SerializeField] private float _doubleSideOutOfRoadEnemyChance; // tower on two side, for example;
        [Range(0, 1)]
        [SerializeField] private float _maxFullnesRoadPartChance;
        [Space]
        [SerializeField] private List<EnemiesFullness> _enemiesFullness;

        public LevelConfigData GetData()
        {
            var data = new LevelConfigData();

            data.RoadPartsCount = _roadPartCount;
            data.MaxMovableEnemyiesInGroup = _maxMovableEnemyiesInGroup;
            data.MinMovableEnemyiesInGroup = _minMovableEnemyiesInGroup;
            data.MaxOnShelterEnemiesInGroup = _maxOnShelterEnemiesInGroup;
            data.MinOnShelterEnemiesInGroup = _minOnShelterEnemiesInGroup;

            data.MinSolderLevel = _minSolderLevel;
            data.MaxSolderLevel = _maxSolderLevel;

            data.DoubleSideOutOfRoadEnemyChance = _doubleSideOutOfRoadEnemyChance;
            data.MaxFullnesRoadPartChance = _maxFullnesRoadPartChance;

            data.EnemiesFullness = _enemiesFullness;

            return data;
        }

    }

    [Serializable]
    public class EnemiesFullness
    {
        [SerializeField] private EnemyType _type;
        [Range(0, 100)]
        [SerializeField] private int _chance;

        public EnemyType EnemyType => _type;
        public int Chance => _chance;
    }
}
