using Assets.Scripts.Characters;
using System;
using UnityEngine;

namespace Assets.Scripts.Guns
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        private const float Lifetime = 5f;

        private Rigidbody _rigidbody;

        private bool _isActive = true;
        private Enemy _enemyHited;

        public event Action<Bullet, Enemy> Hited;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isActive)
            {
                collision.gameObject.TryGetComponent(out _enemyHited);
                Deactivate();
            }
        }

        public void Activate(Vector3 position, Vector3 moveDirection)
        {
            _enemyHited = null;

            transform.position = position;
            transform.LookAt(moveDirection);

            _isActive = true;

            gameObject.SetActive(true);

            _rigidbody.AddForce(moveDirection.normalized * 10f, ForceMode.Impulse);

            //Utils.WaitSeconds(Lifetime)
            //  .Then(() =>
            //  {
            //      if (gameObject != null && gameObject.activeInHierarchy)
            //          Deactivate();
            //  });
        }

        private void Deactivate()
        {
            _isActive = false;
            Hited?.Invoke(this, _enemyHited);
        }
    }
}
