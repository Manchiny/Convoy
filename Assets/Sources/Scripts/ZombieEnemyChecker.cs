using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class ZombieEnemyChecker : MonoBehaviour
    {
        private Enemy _zombie;

        private void Awake()
        {
            _zombie = GetComponentInParent<Enemy>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IEnemyTarget target))
                _zombie.OnAttackableEnter(target);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IEnemyTarget target))
                _zombie.OnAttackableExit(target);
        }
    }
}
