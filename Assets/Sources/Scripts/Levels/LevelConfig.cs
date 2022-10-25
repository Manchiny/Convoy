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
        [SerializeField] private List<EnemiesFullness> _enemiesFullness;

        private const int MaxChanse = 100;

        private System.Random _random;

        public int RoadPartsCount => _roadPartCount;
        public int MaxMovableEnemyiesInGroup => _maxMovableEnemyiesInGroup;
        public int MinMovableEnemyiesInGroup => _minMovableEnemyiesInGroup;
        public IReadOnlyList<EnemiesFullness> EnemiesFullness => _enemiesFullness;

        public EnemyType GetRandomEnemyType()
        {
            _random = new System.Random();
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
