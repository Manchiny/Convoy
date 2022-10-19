using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class EnemyChecker : MonoBehaviour
    {
        private Player _player;

        private void Awake()
        {
            _player = GetComponentInParent<Player>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy zombie))
                _player.OnEnemyDetected(zombie);

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Enemy zombie))
                _player.OnEnemyOutDetected(zombie);
        }
    }
}
