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
        private bool NeedAttack => Target != null && (Target.transform.position - transform.position).sqrMagnitude <= AttackDistance * AttackDistance;

        protected override void Awake()
        {
            base.Awake();
            _agent = GetComponent<NavMeshAgent>();
            _collider = GetComponent<Collider>();
        }

        private void Update()
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
            if (Target.IsAlive == false)
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