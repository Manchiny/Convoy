using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform _tankSpawnPoint;
        [SerializeField] private Transform _playerSpawnPoint;

        [SerializeField] private List<RoadPart> _roadPrefabs;
        [SerializeField] private LevelConfig _config;

        private List<RoadPart> _currentRoad = new();

        public IReadOnlyList<Vector3> Waypoints => _currentRoad.Select(road => road.Center).ToList();
        public Transform TankSpawnPoint => _tankSpawnPoint;
        public Transform PlayerSpawnPoint => _playerSpawnPoint;

        public void CreateRoad()//(LevelConfig config)
        {
            _currentRoad.ForEach(road => Destroy(road.gameObject));
            _currentRoad.Clear();

            var firstRoadPart = Instantiate(_roadPrefabs[0]);
            firstRoadPart.transform.position = Vector3.zero;
            _currentRoad.Add(firstRoadPart);

            for (int i = 1; i < _config.RoadPartsCount; i++)
            {
                CreatRandomRoadPart();
            }

        }

        private void CreatRandomRoadPart()
        {
            int random = Random.Range(0, _roadPrefabs.Count);
            var prefab = _roadPrefabs[random];

            var newRoadPart = Instantiate(prefab);
            var position = _currentRoad.Last().EndConnectorPosition;
            position.z += RoadPart.Lenght / 2f;

            newRoadPart.transform.position = position;

            _currentRoad.Add(newRoadPart);
        }
    }
}
