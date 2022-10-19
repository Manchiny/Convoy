using Assets.Scripts.Characters;
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

        protected virtual int Damage => 10;

        private void Start()
        {
            CreatePool();
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

            //Utils.WaitSeconds(CooldawnSeconds)
            //    .Then(() => _canShoot = true);
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

        private void OnBulletHited(Bullet bullet, Enemy zombie)
        {
            //if (zombie != null)
            //    Game.Instance.OnZombieHited(zombie, Damage);

            bullet.gameObject.SetActive(false);
            _bulletsPool.Enqueue(bullet);
        }

        private void RemoveSubscribes()
        {
            foreach (var bullet in _bulletsPool)
            {
                bullet.Hited -= OnBulletHited;
            }
        }
    }
}
