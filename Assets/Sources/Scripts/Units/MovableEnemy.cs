using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Collider))]
    public class MovableEnemy : Enemy
    {
        private const float AttackDistance = 14f;

        private NavMeshAgent _agent;
        private Collider _collider;

        public override int MaxHealth => 150;
        public override int Armor => 0;
        public override int Damage => 15;
        public override float ShootDelay => 0.3f;

        private bool NeedAttack => Target != null && (Target.transform.position - transform.position).sqrMagnitude <= AttackDistance * AttackDistance;

        protected override void Awake()
        {
            base.Awake();
            _agent = GetComponent<NavMeshAgent>();
            _collider = GetComponent<Collider>();
        }

        private void FixedUpdate()
        {
            if (IsAlive == false)
                return;

            if (NeedAttack)
                Attack();
            else if (Target != null)
                MoveTo(Target.transform);
            else
                StopAnithing();
        }

        public override void OnRestart()
        {
            base.OnRestart();
            _collider.isTrigger = false;
        }

        protected override void OnGetDamage()
        {
            //  throw new System.NotImplementedException();
        }

        protected override void OnDie()
        {
            _collider.isTrigger = true;
        }

        private void Attack()
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

        private void MoveTo(Transform target)
        {
            _agent.SetDestination(target.position);
            Animations.PlayAnimation(EnemyAnimations.RunAnimationKey);
        }

        private void StopAnithing()
        {
            _agent.ResetPath();
            Animations.PlayAnimation(EnemyAnimations.IdleAnimationKey);
        }
    }
}
