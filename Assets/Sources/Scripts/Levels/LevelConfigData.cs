using System;
using System.Collections.Generic;
using static Assets.Scripts.Units.Enemy;

namespace Assets.Scripts.Levels
{
    [Serializable]
    public class LevelConfigData
    {
        private const int MaxChanse = 100;

        public int RoadPartsCount;

        public int MaxMovableEnemyiesInGroup;
        public int MinMovableEnemyiesInGroup;
        public int MaxOnShelterEnemiesInGroup;
        public int MinOnShelterEnemiesInGroup;

        public int MinSolderLevel;
        public int MaxSolderLevel;

        public float DoubleSideOutOfRoadEnemyChance;
        public float MaxFullnesRoadPartChance;

        public List<EnemiesFullness> EnemiesFullness;

        private Random _systemRandom = new Random();

        public int GetRandomMovableEnimiesInGroupCount => _systemRandom.Next(MinMovableEnemyiesInGroup, MaxMovableEnemyiesInGroup);
        public int GetRandomInShelterEnemiesInGroupCount => _systemRandom.Next(MinOnShelterEnemiesInGroup, MaxOnShelterEnemiesInGroup);
        public int GetRandomUnitLevel => _systemRandom.Next(MinSolderLevel, MaxSolderLevel);

        public EnemyType GetRandomEnemyType()
        {
            int random = _systemRandom.Next(0, MaxChanse);

            int chanseCounter = 0;

            for (int i = 0; i < EnemiesFullness.Count; i++)
            {
                EnemiesFullness fullnes = EnemiesFullness[i];

                if (random > chanseCounter && random <= chanseCounter + fullnes.Chance)
                    return fullnes.EnemyType;

                chanseCounter += fullnes.Chance;
            }

            return EnemyType.Movable;
        }

        public List<EnemyType> GetRandomTypesForFullFill(List<EnemyType> outOfRoadTypes)
        {
            List<EnemyType> result = new();

            List<EnemiesFullness> outOfRoad = new();
            List<EnemiesFullness> onRoad = new();

            foreach (var enemy in EnemiesFullness)
            {
                if (outOfRoadTypes.Contains(enemy.EnemyType))
                    outOfRoad.Add(enemy);
                else
                    onRoad.Add(enemy);
            }

            if (outOfRoad.Count > 0)
            {
                result.Add(GetRandomFromList(outOfRoad, true));
                result.Add(GetRandomFromList(outOfRoad, true));
            }

            if (onRoad.Count > 0)
                result.Add(GetRandomFromList(onRoad, false));

            return result;
        }

        private EnemyType GetRandomFromList(List<EnemiesFullness> enemieFullnes, bool isOutOfRoad)
        {
            int maxChance = 0;

            foreach (var enemy in EnemiesFullness)
                if (enemieFullnes.Contains(enemy))
                    maxChance += enemy.Chance;

            int random = _systemRandom.Next(0, maxChance);

            int chanseCounter = 0;

            for (int i = 0; i < enemieFullnes.Count; i++)
            {
                EnemiesFullness fullnes = enemieFullnes[i];

                if (random > chanseCounter && random <= chanseCounter + fullnes.Chance)
                    return fullnes.EnemyType;

                chanseCounter += fullnes.Chance;
            }

            if (isOutOfRoad)
                return EnemyType.Tower;

            return EnemyType.Movable;
        }
    }
}
