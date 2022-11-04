using UnityEngine;

namespace Assets.Scripts
{
    public class Angar : MonoBehaviour
    {
        [SerializeField] private Transform _tankEndPoint;
        [SerializeField] float _length = 67f;

        public float Length => _length;
        public Vector3 TankEndPoint => _tankEndPoint.position;
    }
}