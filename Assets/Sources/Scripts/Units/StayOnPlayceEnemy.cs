using UnityEngine;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(Collider))]
    public class StayOnPlayceEnemy : Enemy, IEnemyGroupable
    {
        public EnemyGroup _group;

        protected virtual float AttackDistance => 24f;

        public override Damageable Target => _group.Target;
        public EnemyGroup Group => _group;

        protected bool NeedAttack => Target != null && (Target.transform.position - transform.position).sqrMagnitude <= AttackDistance * AttackDistance;


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
            Collider.isTrigger = false;
            _group.ResetTarget();
        }

        protected override void OnEnemyFinded(Damageable enemy)
        {
            _group.OnAnyFindTarget(enemy);
        }

        protected override void OnEnenmyMissed(Damageable enemy) { }

        protected override void OnDie()
        {
            Collider.isTrigger = true;
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
