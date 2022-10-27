using UnityEngine;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(Collider))]
    public class StayOnPlayceEnemy : Enemy, IEnemyGroupable
    {
        private Collider _collider;
        public EnemyGroup _group;

        public override int MaxHealth => 150;
        public override int Armor => 0;
        public override int Damage => 15;
        public override float ShootDelay => 0.3f;

        protected virtual float AttackDistance => 24f;

        public override Damageable Target => _group.Target;
        public EnemyGroup Group => _group;

        protected bool NeedAttack => Target != null && (Target.transform.position - transform.position).sqrMagnitude <= AttackDistance * AttackDistance;

        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<Collider>();
        }

        private void FixedUpdate()
        {
            if (IsAlive == false || Group == null)
                return;

            if (NeedAttack)
                Attack();
        }

        public void SetGroup(EnemyGroup group)
        {
            _group = group;
        }

        public override void OnRestart()
        {
            base.OnRestart();
            _collider.isTrigger = false;
            _group.ResetTarget();
        }

        protected override void OnEnemyFinded(Damageable enemy)
        {
            _group.OnAnyFindTarget(enemy);
        }

        protected override void OnEnenmyMissed(Damageable enemy) { }

        protected override void OnDie()
        {
            _collider.isTrigger = true;
        }

        protected virtual void Attack()
        {
            if (Target.gameObject.activeInHierarchy == false || Target.IsAlive == false)
                RemoveFromEnemies(Target);
            else
            {
                transform.LookAt(Target.transform);
                TryShoot();
                Animations.PlayAnimation(EnemyAnimations.AttackAnimationKey);
            }
        }
    }
}
