using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class Enemy : Character
    {
        private const string IdleAnimationKey = "Idle";
        private const string RunAnimationKey = "Run";
        private const string AttackAnimationKey = "Idle";

        private const float AttackDistance = 5f;

        private string _lastAnimationKey;

        private NavMeshAgent _agent;
        private Animator _animator;

        public override int MaxHealth => 150;
        public virtual int Damage => 10;

        private bool NeedAttack => Target != null && (Target.transform.position - transform.position).sqrMagnitude <= AttackDistance * AttackDistance;

        public override Team TeamId => Team.Enemy;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
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

        protected override void Die()
        {
            gameObject.SetActive(false);
        }

        protected override void OnEnemyFinded(Damageable enemy)
        {
            if (Target == null)
                Target = enemy;
        }

        protected override void OneEnenmyMissed(Damageable enemy)
        {
            if (enemy == Target || (Target != null && Target.IsAlive == false))
                Target = TryGetAnyNewTarget();
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
                PlayAnimation(AttackAnimationKey);
            }
        }

        private void MoveTo(Transform target)
        {
            _agent.SetDestination(target.position);
            PlayAnimation(RunAnimationKey);
        }

        private void StopAnithing()
        {
            _agent.ResetPath();
            PlayAnimation(IdleAnimationKey);
        }

        private void PlayAnimation(string animationKey)
        {
            if (_lastAnimationKey == animationKey)
                return;

            _lastAnimationKey = animationKey;

            _animator.StopPlayback();
            _animator.CrossFade(animationKey, 0.1f);
        }
    }
}
