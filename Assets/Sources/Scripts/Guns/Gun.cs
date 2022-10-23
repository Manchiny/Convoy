using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Damageable;

namespace Assets.Scripts.Guns
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private Bullet _bullet;
        [SerializeField] private ParticleSystem _shootingEffect;
        [SerializeField] private int _poolCount = 20;

        private Vector3 _shootingDiredtionOffaset = new Vector3(0, 1, 0);
        private Queue<Bullet> _bulletsPool = new();

        private WaitForSeconds _waitSeconds;
        private Coroutine _cooldawnAwaite;

        protected virtual int Damage => 10;
        protected virtual float CooldawnSeconds => 0.3f;

        protected Transform ShootingPoint => _shootingPoint;
        protected bool CanShoot { get; set; } = true;

        private void Start()
        {
            CreatePool();
            _waitSeconds = new WaitForSeconds(CooldawnSeconds);
        }

        private void OnDisable()
        {
            RemoveSubscribes();
        }

        private void OnDestroy()
        {
            DestroyPool();
        }

        public virtual void TryShoot(Damageable target, Team team)
        {
            if (!CanShoot)
                return;

            Vector3 direction = target.transform.position + _shootingDiredtionOffaset - _shootingPoint.transform.position;
            Shoot(direction, team);
        }

        protected void Shoot(Vector3 direction, Team team)
        {
            CanShoot = false;

            var bullet = GetFreeBullet();
            bullet.Activate(_shootingPoint.position, direction, team, this);

            _shootingEffect.Play();

            if (_cooldawnAwaite != null)
                StopCoroutine(_cooldawnAwaite);

            _cooldawnAwaite = StartCoroutine(WaitCooldawn());
        }

        private IEnumerator WaitCooldawn()
        {
            yield return _waitSeconds;
            CanShoot = true;
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
            for (int i = 0; i < _poolCount; i++)
            {
                CreateBulletForPool();
            }
        }

        private void CreateBulletForPool()
        {
            Bullet bullet = Instantiate(_bullet, _shootingPoint.position, Quaternion.identity, Game.GarbageHolder);
            bullet.gameObject.SetActive(false);

            bullet.Hited += OnBulletHited;

            _bulletsPool.Enqueue(bullet);
        }

        private void OnBulletHited(Bullet bullet, Damageable damageable)
        {
            if (damageable != null)
                Game.Instance.OnDamageableHited(damageable, Damage);

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

        private void DestroyPool()
        {
            while(_bulletsPool.Count > 0)
            {
                Bullet bullet = _bulletsPool.Dequeue();

                if(bullet !=null)
                    Destroy(bullet.gameObject);
            }
        }
    }
}
