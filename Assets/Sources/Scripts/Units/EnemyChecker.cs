using UnityEngine;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyChecker : MonoBehaviour
    {
        [SerializeField] EnemyCheckerRenderer _renderer;
        private IAttackable _character;

        private void Awake()
        {
            _character = GetComponentInParent<IAttackable>();

            if (_renderer != null)
                _renderer.SetRadius(GetComponent<SphereCollider>().radius * transform.localScale.x);
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
