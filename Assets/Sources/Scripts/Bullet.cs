using System;
using System.Collections;
using UnityEngine;
using static Assets.Scripts.Damageable;

namespace Assets.Scripts.Guns
{
    public class Bullet : MonoBehaviour
    {
        private const float Lifetime = 5f;
        private const float Speed = 10f;

        private Team _team;

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

        private void Update()
        {
            if (_isActive == false)
                return;

            transform.Translate(_moveDirection * Time.deltaTime * Speed);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isActive)
                Deactivate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isActive)
            {
                if (other.gameObject.TryGetComponent(out Damageable enemyHited) && enemyHited.TeamId != _team)
                {
                    _enemyHited = enemyHited;
                    Deactivate();
                }
            }
        }

        public void Activate(Vector3 position, Vector3 moveDirection, Team team)
        {
            _enemyHited = null;
            _team = team;

            transform.position = position;
            // transform.LookAt(moveDirection);
            _moveDirection = moveDirection;

            _isActive = true;

            gameObject.SetActive(true);

        //    _rigidbody.AddForce(moveDirection.normalized * 10f, ForceMode.Impulse);

            _autoDeactivate = StartCoroutine(DeactivatAfterLifetime());
        }

        private void Deactivate()
        {
            if (_autoDeactivate != null)
                StopCoroutine(_autoDeactivate);

            _isActive = false;
            Hited?.Invoke(this, _enemyHited);
        }

        private IEnumerator DeactivatAfterLifetime()
        {
            yield return _waitSeconds;

            if (gameObject != null && gameObject.activeInHierarchy)
                Deactivate();
        }
    }
}
