using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Collider))]
    public class MovableEnemy : Enemy, IEnemyGroupable
    {
        private NavMeshAgent _agent;
        private Collider _collider;
        private EnemyGroup _group;

        public override int MaxHealth => 150;
        public override int Armor => 0;
        public override int Damage => 15;
        public override float ShootDelay => 0.3f;

        protected virtual float AttackDistance => 14f;

        public override Damageable Target  => _group.Target;
        public EnemyGroup Group => _group;

        protected bool NeedAttack => Target != null && (Target.transform.position - transform.position).sqrMagnitude <= AttackDistance * AttackDistance;

        protected override void Awake()
        {
            base.Awake();
            _agent = GetComponent<NavMeshAgent>();
            _collider = GetComponent<Collider>();
        }

        private void FixedUpdate()
        {
            if (IsAlive == false || _group == null)
                return;

            OnFixedUpdate();
        }

        //private void OnEnable()
        //{
        //    StayOnPlace();
        //}

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

        protected virtual void OnFixedUpdate()
        {
            if (NeedAttack)
                Attack();
            else if (Target != null && Target is Tank)
                MoveTo(Target.transform);
            else
                StayOnPlace();
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

        protected void Attack()
        {
            if (Target.gameObject.activeInHierarchy == false || Target.IsAlive == false)
                RemoveFromEnemies(Target);
            else
            {
                _agent.ResetPath();

                transform.LookAt(Target.transform);
                TryShoot();
                Animations.PlayAnimation(EnemyAnimations.AttackAnimationKey);
            }
        }

        protected void MoveTo(Transform target)
        {
            _agent.SetDestination(target.position);
            Animations.PlayAnimation(EnemyAnimations.RunAnimationKey);
        }

        protected void StayOnPlace()
        {
            _agent.ResetPath();
            Animations.PlayAnimation(EnemyAnimations.IdleAnimationKey);
        }
    }
}
