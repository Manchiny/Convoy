using UnityEngine;

namespace Assets.Scripts.Characters
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyChecker : MonoBehaviour
    {
        [SerializeField] EnemyCheckerRenderer _renderer;
        private Character _character;

        private void Awake()
        {
            _character = GetComponentInParent<Character>();

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
