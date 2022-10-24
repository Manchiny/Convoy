using Assets.Scripts.Guns;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public abstract class Unit : Damageable, IAttackable
    {
        [SerializeField] protected Gun Gun;

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
            OnEnenmyMissed(enemy);
        }

        protected abstract void OnEnemyFinded(Damageable enemy);
        protected abstract void OnEnenmyMissed(Damageable enemy);

        protected void TryShoot()
        {
            if (Target.gameObject.activeInHierarchy == false || Target.IsAlive == false)
                RemoveFromEnemies(Target);

            if (Target == null)
                return;

            Gun.TryShoot(Target, TeamId);
        }

        protected Damageable TryGetAnyNewTarget()
        {
            if (_attackTargets.Count > 0)
                return _attackTargets.FirstOrDefault();
            else
                return null;
        }

        protected Damageable TryGetNearestTarget()
        {
            if (_attackTargets.Count == 0)
                return null;

            return _attackTargets.Where(enemy => enemy.gameObject.activeInHierarchy).OrderBy(enemy => (enemy.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
        }
    }
}
