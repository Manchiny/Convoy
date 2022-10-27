using System;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Units.Enemy;

namespace Assets.Scripts.Levels
{
    [CreateAssetMenu]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private int _roadPartCount;
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
        [Range(0,4)]
        [SerializeField] private int _minSolderLevel = 0;
        [Range(0, 4)]
        [SerializeField] private int _maxSolderLevel = 0;
        [Space]
        [SerializeField] private List<EnemiesFullness> _enemiesFullness;

        private const int MaxChanse = 100;

        private System.Random _random = new System.Random();

        public int RoadPartsCount => _roadPartCount;

        public IReadOnlyList<EnemiesFullness> EnemiesFullness => _enemiesFullness;

        public int GetRandomMovableEnimiesInGroupCount => _random.Next(_minMovableEnemyiesInGroup, _maxMovableEnemyiesInGroup);
        public int GetRandomInShelterEnemiesInGroupCount => _random.Next(_minOnShelterEnemiesInGroup, _maxOnShelterEnemiesInGroup);
        public int GetRandomUnitLevel => _random.Next(_minSolderLevel, _maxSolderLevel);

        public EnemyType GetRandomEnemyType()
        {
            int random = _random.Next(0, MaxChanse);

            int chanseCounter = 0;

            for (int i = 0; i < _enemiesFullness.Count; i++)
            {
                EnemiesFullness fullnes = _enemiesFullness[i];

                if (random > chanseCounter && random <= chanseCounter + fullnes.Chance)
                    return fullnes.EnemyType;

                chanseCounter += fullnes.Chance;
            }

            return EnemyType.Movable;
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
