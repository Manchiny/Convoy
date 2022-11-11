using System;
using System.Collections;
using UnityEngine;
using static Assets.Scripts.Damageable;

namespace Assets.Scripts.Guns
{
    public class Bullet : MonoBehaviour
    {
        protected virtual float Lifetime => 5f;
        protected virtual float Speed => 10f;

        private Team _team;
        private Gun _gun;

        private bool _isActive = true;
        private Damageable _enemyHited;

        private WaitForSeconds _waitSeconds;
        private Coroutine _autoDeactivate;

        private Vector3 _moveDirection;

        public event Action<Bullet, Damageable> Hited;

        private void Awake()
        {
            _waitSeconds = new WaitForSeconds(Lifetime);
        }

        private void FixedUpdate()
        {
            if (_isActive == false)
                return;

            transform.Translate(_moveDirection * Time.fixedDeltaTime * Speed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isActive)
            {
                if (other.gameObject.TryGetComponent(out Damageable enemyHited) && enemyHited.TeamId != _team && enemyHited.IsAlive)
                {
                    _enemyHited = enemyHited;
                    Deactivate();
                }
            }
        }

        public void Activate(Vector3 position, Vector3 moveDirection, Team team, Gun gun)
        {
            _enemyHited = null;
            _team = team;
            _gun = gun;

            transform.position = position;
            _moveDirection = moveDirection;

            _isActive = true;

            gameObject.SetActive(true);
            _autoDeactivate = StartCoroutine(DeactivatAfterLifetime());
        }

        private void Deactivate()
        {
            if (_autoDeactivate != null)
                StopCoroutine(_autoDeactivate);

            _isActive = false;

            Hited?.Invoke(this, _enemyHited);
            
            if (_gun == null || _gun.gameObject == null)
                Destroy(this);
        }

        private IEnumerator DeactivatAfterLifetime()
        {
            yield return _waitSeconds;

            if (gameObject != null && gameObject.activeInHierarchy)
                Deactivate();
        }
    }
}
