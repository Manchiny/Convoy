using Assets.Scripts.Guns;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public abstract class Character : Damageable
    {
        [SerializeField] private Gun _gun;

        private HashSet<Damageable> _attackTargets = new();

        public Damageable Target { get; protected set; }

        public void AddFindedEnemy(Damageable enemy)
        {
            _attackTargets.Add(enemy);
            OnEnemyFinded(enemy);
        }

        public void RemoveFromEnemies(Damageable enemy)
        {
            _attackTargets.Remove(enemy);
            OneEnenmyMissed(enemy);
        }

        protected abstract void OnEnemyFinded(Damageable enemy);
        protected abstract void OneEnenmyMissed(Damageable enemy);

        protected void TryShoot()
        {
            if (Target.IsAlive == false)
                RemoveFromEnemies(Target);

            if (Target == null)
                return;

            _gun.TryShoot(Target, TeamId);
        }

        protected Damageable TryGetAnyNewTarget()
        {
            if (_attackTargets.Count > 0)
                return _attackTargets.First();
            else
                return null;
        }

        protected Damageable TryGetNearestTarget()
        {
            if (_attackTargets.Count == 0)
                return null;

            return _attackTargets.OrderBy(enemy => (enemy.transform.position - transform.position).sqrMagnitude).First();
        }
    }
}
