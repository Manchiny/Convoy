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

        private IAttackable _attackable;
      //  private WaitForSeconds _waitSeconds;
        private Coroutine _cooldawnAwaite;

        protected int Damage => _attackable.Damage;
        protected virtual float CooldawnSeconds => _attackable.ShootDelay;

        protected Transform ShootingPoint => _shootingPoint;
        protected bool CanShoot { get; set; } = true;

        private void Awake()
        {
            _attackable = GetComponentInParent<IAttackable>();
        }

        private void Start()
        {
            CreatePool();
        }

        private void OnDisable()
        {
            if (_cooldawnAwaite != null)
                StopCoroutine(_cooldawnAwaite);
        }

        private void OnDestroy()
        {
            DestroyPool();
            RemoveSubscribes();
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
            yield return new WaitForSeconds(CooldawnSeconds);
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
