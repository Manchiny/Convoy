using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovableEnemy :  StayOnPlayceEnemy
    {
        private NavMeshAgent _agent;
        protected override float AttackDistance => 14f;


        protected override void Awake()
        {
            base.Awake();
            _agent = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            if (IsAlive == false || Group == null)
                return;

            OnFixedUpdate();
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

        protected override void Attack()
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
