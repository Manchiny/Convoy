using Assets.Scripts.Guns;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : Character
    {
        [SerializeField] private Gun _gun;

        private HashSet<Enemy> _enemies = new();

        public Enemy Target { get; private set; }
        public PlayerMovement Movement { get; private set; }

        public Damageable Damageable => this;

        public override int MaxHealth => 100;

        private void Awake()
        {
            Movement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
           // if (Target != null)
                TryShoot();
        }

        public void OnEnemyDetected(Enemy enemy)
        {
            _enemies.Add(enemy);

            if (Target == null)
                Target = enemy;
        }

        public void OnEnemyOutDetected(Enemy enemy)
        {
            _enemies.Remove(enemy);

            if (enemy == Target || (Target != null && Target.IsAlive == false))
                Target = TryGetNewTarget();
        }

        protected override void OnGetDamage()
        {
            Debug.Log("Player take damage");
        }

        protected override void Die()
        {
            Debug.LogWarning("Game over");
        }

        private void TryShoot()
        {
            //if (Target.IsAlive == false)
            //{
            //    _enemies.Remove(Target);
            //    Target = TryGetNewTarget();
            //}

            //if (Target == null)
            //    return;

            _gun.TryShoot(Target);
        }

        private Enemy TryGetNewTarget()
        {
            if (_enemies.Count > 0)
                return _enemies.First();
            else
                return  null;
        }
    }
}
