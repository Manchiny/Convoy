using Assets.Scripts.Items;
using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Assets.Scripts.Units.Enemy;

namespace Assets.Scripts.Levels
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private Transform _tankSpawnPoint;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _roadContainer;
        [SerializeField] private Transform _enemiesContainer;
        [Space]
        [SerializeField] private List<RoadPart> _roadPrefabs;
        [SerializeField] private LevelsDatabase _levelsDatabase;
        [Space]
        [SerializeField] private List<EnemyPrefab> _enemyPrefabs;
        [SerializeField] private Ground _ground;
        [SerializeField] private Transform _borderEnd;
        [SerializeField] private EndLevelCheckpoint _endLevelCheckpoint;
        [Space]
        [SerializeField] private AirPlane _airPlane;
        [SerializeField] private Angar _finishAngar;

        private readonly Vector3 EnemyRotation = new Vector3(0, -180, 0);
        private readonly Vector3 AirDropOffsetPosition = new Vector3(13.5f, 0,0);
        private readonly Vector3 AirplaneOffsetStartPosition = new Vector3(-1, 40, -5);

        private List<RoadPart> _currentRoad = new();
        private System.Random _systemRandom = new();

        private List<GameDrop> _drops = new();

        private RoadPart _lastFilledRoadPart;
        private float _lastOffsetXOnRoad;

        private List<Vector3> _waypoints = new();
        public IReadOnlyList<Vector3> Waypoints => _waypoints;
        public Transform TankSpawnPoint => _tankSpawnPoint;
        public Transform PlayerSpawnPoint => _playerSpawnPoint;
        public float DatabaseVesrsion => _levelsDatabase.Version;

        public void InitData(LevelsDatabaseData data)
        {
            _levelsDatabase.Init(data);
        }

        public void LoadLevel(int levelId)
        {
            LevelConfigData config = _levelsDatabase.GetLevelConfig(levelId);
            _waypoints.Clear();

            Configure(config);

            if (_drops.Count > 0)
                foreach (var drop in _drops)
                    Destroy(drop.gameObject);

            _drops.Clear();
        }

        public void CreateAirDrop(List<ItemCount> items)
        {
            var roadPart = _currentRoad[1];

            Vector3 dropPosition = roadPart.Center + AirDropOffsetPosition;
            _airPlane.transform.position = roadPart.Center + AirplaneOffsetStartPosition;
            _airPlane.gameObject.SetActive(true);

            _drops.Add(_airPlane.DeliveDrop(dropPosition, items, true));
        }

        private void Configure(LevelConfigData config)
        {
            RemoveOldRoad();
            CreateRoad(config);

            _finishAngar.transform.position = _currentRoad.Last().EndConnectorPosition + new Vector3(0, 0, _finishAngar.Length / 2f);
            _waypoints.Add(_finishAngar.TankEndPoint);

            _endLevelCheckpoint.transform.position = _waypoints.Last();

            _ground.Resize(_currentRoad.Count * RoadPart.Lenght);

            RemoveOldEnemies();
            CreateEnemyies(config);

            _borderEnd.position = _currentRoad.Last().EndConnectorPosition;
        }

        private void CreateRoad(LevelConfigData config)
        {
            var firstRoadPart = Instantiate(_roadPrefabs[0], _roadContainer);
            firstRoadPart.transform.position = Vector3.zero;
            _currentRoad.Add(firstRoadPart);

            for (int i = 1; i < config.RoadPartsCount; i++)
                 CreatRandomRoadPart();
        }

        private void RemoveOldRoad()
        {
            _currentRoad.ForEach(road => Destroy(road.gameObject));
            _currentRoad.Clear();
        }

        private void CreateEnemyies(LevelConfigData config)
        {
            for (int i = 2; i < _currentRoad.Count - 1; i++)
            {
                RoadPart road = _currentRoad[i];

                bool needDoubleSide = false;
                bool needFillMax = _systemRandom.NextDouble() <= config.MaxFullnesRoadPartChance;

                Damageable prefab = null;

                if (needFillMax)
                {
                    List<EnemyType> outOfRoadTypes = _enemyPrefabs.Where(enemy => enemy.Prefab is IOutOfRoad).Select(enemy => enemy.EnemyType).ToList();
                    List<EnemyType> types = config.GetRandomTypesForFullFill(outOfRoadTypes);

                    foreach (var type in types)
                    {
                        prefab = _enemyPrefabs.Where(enemy => enemy.EnemyType == type).First().Prefab;
                        CreateEnemy(type, prefab, road, config, false, true);
                    }
                }
                else
                {
                    EnemyType type = config.GetRandomEnemyType();
                    prefab = _enemyPrefabs.Where(enemy => enemy.EnemyType == type).First().Prefab;

                    if (prefab is IOutOfRoad)
                        needDoubleSide = _systemRandom.NextDouble() <= config.DoubleSideOutOfRoadEnemyChance;

                    CreateEnemy(type, prefab, road, config, needDoubleSide);
                }
            }
        }

        private void CreateEnemy(EnemyType type, Damageable prefab, RoadPart road, LevelConfigData config, bool needDoubleSide = false, bool needCountLastOffset = false)
        {
            switch (type)
            {
                case EnemyType.Movable:
                    CreateMovableGroup(config, road, prefab);
                    break;
                case EnemyType.Tower:
                    CreateTower(config, road, prefab, needDoubleSide, needCountLastOffset);
                    break;
                case EnemyType.RoadShelter:
                    CreateRoadShelter(config, road, prefab);
                    break;
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
            _waypoints.Add(newRoadPart.Center);
        }

        private void CreateMovableGroup(LevelConfigData config, RoadPart road, Damageable prefab)
        {
            int count = config.GetRandomMovableEnimiesInGroupCount;
            float maxRandomPositionDistance = 5f;

            EnemyGroup group = new();

            for (int i = 0; i < count; i++)
            {
                Damageable enemy = Instantiate(prefab, _enemiesContainer);
                enemy.transform.position = GetRandomPosition();
                enemy.transform.rotation = Quaternion.Euler(EnemyRotation);

                TryLoadUnitData(enemy, config);

                if (enemy is IEnemyGroupable)
                {
                    var groupable = enemy as IEnemyGroupable;
                    groupable.SetGroup(group);
                }
                else
                    Debug.LogError("You are trying to create enemy group, but prefab is not IEnemyGroupable!");
            }

            Vector3 GetRandomPosition()
            {
                Vector3 position = UnityEngine.Random.insideUnitSphere * maxRandomPositionDistance;
                position.y = 0;

                return position + road.Center;
            }
        }

        private void CreateTower(LevelConfigData config, RoadPart road, Damageable prefab, bool needDoubleSide = false, bool needCountLastOffset = false)
        {
            int random = _systemRandom.Next(0, 100);
            float offsetX = random < 50 ? -RoadPart.TowerOffset : RoadPart.TowerOffset;

            if (needCountLastOffset && road == _lastFilledRoadPart)
                offsetX = (-1) * _lastOffsetXOnRoad;

            Vector3 position = road.Center;
            position.x += offsetX;

            Quaternion rotation = Quaternion.Euler(EnemyRotation);

            Tower tower = Instantiate(prefab, position, rotation, _enemiesContainer) as Tower;
            TryLoadUnitData(tower.Unit, config);

            if (needDoubleSide)
            {
                float offset = (-1) * offsetX;
                position.x = road.Center.x + offset;

                Tower towerSecond = Instantiate(prefab, position, rotation, _enemiesContainer) as Tower;
                TryLoadUnitData(towerSecond.Unit, config);
            }

            _lastFilledRoadPart = road;
            _lastOffsetXOnRoad = offsetX;
        }

        private void CreateRoadShelter(LevelConfigData config, RoadPart road, Damageable prefab)
        {
            int soldersCount = config.GetRandomInShelterEnemiesInGroupCount;

            Vector3 position = road.Center;
            Quaternion rotation = Quaternion.Euler(EnemyRotation);

            Damageable enemy = Instantiate(prefab, position, rotation, _enemiesContainer);

            if (enemy is RoadShelter)
            {
                var shelter = enemy as RoadShelter;
                shelter.CreateSolders(soldersCount, config, _levelsDatabase.SolderLevelDatabase);
            }
            else
                Debug.LogError("You are trying to create enemy group, but prefab is not IEnemyGroupable!");
        }

        private void TryLoadUnitData(Damageable enemy, LevelConfigData config)
        {
            if (enemy is Unit)
            {
                Unit unit = enemy as Unit;
                unit.InitData(new UnitData(config.GetRandomUnitLevel), _levelsDatabase.SolderLevelDatabase);
            }
        }

        private void RemoveOldEnemies()
        {
            var enemies = _enemiesContainer.GetChildrensWithInactive().Where(t => t != _enemiesContainer);

            foreach (var enemy in enemies)
                Destroy(enemy.gameObject);
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
