using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private List<Transform> _waypoints;
        [SerializeField] private Transform _spawnPoint;

        public IReadOnlyList<Vector3> Waypoints => _waypoints.Select(point => point.position).ToList();
        public Transform SpawnPoint => _spawnPoint;
    }
}
