using Assets.Scripts.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Guns
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private Bullet _bullet;
        [SerializeField] private ParticleSystem _shootingEffect;

        private const float CooldawnSeconds = 0.3f;
        private const int PoolCount = 50;

        private Vector3 _offset = new Vector3(0, 1, 0);
        private bool _canShoot = true;

        private Queue<Bullet> _bulletsPool = new();

        private WaitForSeconds _waitSeconds;
        private Coroutine _cooldawnAwaite;

        protected virtual int Damage => 10;

        private void Start()
        {
            CreatePool();
            _waitSeconds = new WaitForSeconds(CooldawnSeconds);
        }

        private void OnDisable()
        {
            RemoveSubscribes();
        }

        public void TryShoot(Enemy target)
        {
            if (!_canShoot)
                return;

            Vector3 direction = transform.forward;// target.transform.position + _offset - _shootingPoint.transform.position;
            Shoot(_shootingPoint.position, direction);
        }

        private void Shoot(Vector3 position, Vector3 direction)
        {
            _canShoot = false;

            var bullet = GetFreeBullet();
            bullet.Activate(position, direction);

            _shootingEffect.Play();

            if (_cooldawnAwaite != null)
                StopCoroutine(_cooldawnAwaite);

            _cooldawnAwaite = StartCoroutine(WaitCooldawn());
        }

        private IEnumerator WaitCooldawn()
        {
            yield return _waitSeconds;
            _canShoot = true;
        }

        private Bullet GetFreeBullet()
        {
            if (_bulletsPool.Count > 0)
                return _bulletsPool.Dequeue();
            else
            {
                CreateBulletForPool();
                return GetFreeBullet();
            }
        }

        private void CreatePool()
        {
            for (int i = 0; i < PoolCount; i++)
            {
                CreateBulletForPool();
            }
        }

        private void CreateBulletForPool()
        {
            Bullet bullet = Instantiate(_bullet, _shootingPoint.position, Quaternion.identity);
            bullet.gameObject.SetActive(false);

            bullet.Hited += OnBulletHited;

            _bulletsPool.Enqueue(bullet);
        }

        private void OnBulletHited(Bullet bullet, Enemy enemy)
        {
            if (enemy != null)
                Game.Instance.OnEnemyHited(enemy, Damage);

            bullet.gameObject.SetActive(false);
            _bulletsPool.Enqueue(bullet);
        }

        private void RemoveSubscribes()
        {
            foreach (var bullet in _bulletsPool)
            {
                bullet.Hited -= OnBulletHited;
            }

            if (_cooldawnAwaite != null)
                StopCoroutine(_cooldawnAwaite);
        }
    }
}
