using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class EnemyChecker : MonoBehaviour
    {
        private Character _character;

        private void Awake()
        {
            _character = GetComponentInParent<Character>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Damageable target) && target.TeamId != _character.TeamId)
                _character.AddFindedEnemy(target);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Damageable target) && target.TeamId != _character.TeamId)
                _character.RemoveFromEnemies(target);
        }
    }
}
