using UnityEngine;

namespace Assets.Scripts.Levels
{
    public class RoadPart : MonoBehaviour
    {
        [SerializeField] private Transform _startConnector;
        [SerializeField] private Transform _endConnector;
        [SerializeField] private Transform _center;

        public const float Lenght = 24f;
        public const float TowerOffset = 13f;

        public Vector3 EndConnectorPosition => _endConnector.position;
        public Vector3 Center => _center.position;

    }
}
