using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Assets.Scripts.Units.Enemy;

namespace Assets.Scripts.Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform _tankSpawnPoint;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _roadContainer;
        [SerializeField] private Transform _enemiesContainer;
        [Space]
        [SerializeField] private List<RoadPart> _roadPrefabs;
        [SerializeField] private LevelConfig _config;
        [Space]
        [SerializeField] private List<EnemyPrefab> _enemyPrefabs;
        [SerializeField] private Ground _ground;

        private readonly Vector3 EnemyRotation = new Vector3(0, -180, 0);

        private List<RoadPart> _currentRoad = new();
        private System.Random _systemRandom = new();

        public IReadOnlyList<Vector3> Waypoints => _currentRoad.Select(road => road.Center).ToList();
        public Transform TankSpawnPoint => _tankSpawnPoint;
        public Transform PlayerSpawnPoint => _playerSpawnPoint;

        public void Configure()//(LevelConfig config)
        {
            CreateRoad();
            _ground.Resize(_currentRoad.Count * RoadPart.Lenght);
            CreateEnemyies();
        }

        private void CreateRoad()
        {
            _currentRoad.ForEach(road => Destroy(road.gameObject));
            _currentRoad.Clear();

            var firstRoadPart = Instantiate(_roadPrefabs[0], _roadContainer);
            firstRoadPart.transform.position = Vector3.zero;
            _currentRoad.Add(firstRoadPart);

            for (int i = 1; i < _config.RoadPartsCount; i++)
            {
                CreatRandomRoadPart();
            }
        }

        private void CreateEnemyies()
        {
            for (int i = 1; i < _currentRoad.Count - 1; i++)
            {
                RoadPart road = _currentRoad[i];
                EnemyType type = _config.GetRandomEnemyType();
                Damageable prefab = _enemyPrefabs.Where(enemy => enemy.EnemyType == type).First().Prefab;

                switch (type)
                {
                    case EnemyType.Movable:
                        CreateMovableGroup(_config, road, prefab);
                        break;
                    case EnemyType.Tower:
                        CreateTower(_config, road, prefab);
                        break;
                }
            }
        }

        private void CreatRandomRoadPart()
        {
            int random = UnityEngine.Random.Range(0, _roadPrefabs.Count);
            var prefab = _roadPrefabs[random];

            var newRoadPart = Instantiate(prefab, _roadContainer);
            var position = _currentRoad.Last().EndConnectorPosition;
            position.z += RoadPart.Lenght / 2f;

            newRoadPart.transform.position = position;

            _currentRoad.Add(newRoadPart);
        }

        private void CreateMovableGroup(LevelConfig config, RoadPart road, Damageable prefab)
        {
            int count = _systemRandom.Next(config.MinMovableEnemyiesInGroup, config.MinMovableEnemyiesInGroup);
            float maxRandomPositionDistance = 5f;

            for (int i = 0; i < count; i++)
            {
                Damageable enemy = Instantiate(prefab, _enemiesContainer);
                enemy.transform.position = GetRandomPosition();
                enemy.transform.rotation = Quaternion.Euler(EnemyRotation);
            }

            Vector3 GetRandomPosition()
            {
                Vector3 position = UnityEngine.Random.insideUnitSphere * maxRandomPositionDistance;
                position.y = 0;

                return position + road.Center;
            }
        }

        private void CreateTower(LevelConfig config, RoadPart road, Damageable prefab)
        {
            int random = _systemRandom.Next(0, 100);
            float offsetX = random < 50 ? -RoadPart.TowerOffset : RoadPart.TowerOffset;

            Vector3 position = road.Center;
            position.x += offsetX;

            Quaternion rotation = Quaternion.Euler(EnemyRotation);

            Instantiate(prefab, position, rotation, _enemiesContainer);
        }
    }

    [Serializable]
    public class EnemyPrefab
    {
        [SerializeField] private EnemyType _type;
        [SerializeField] private Damageable _prefab;

        public EnemyType EnemyType => _type;
        public Damageable Prefab => _prefab;
    }
}
