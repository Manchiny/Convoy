using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Collider))]
    public class Enemy : Unit
    {
        [SerializeField] private SolderBadge _badgePrefab;

        private const string IdleAnimationKey = "Idle";
        private const string RunAnimationKey = "Run";
        private const string AttackAnimationKey = "Idle";
        private const string DeathAnimationKey = "Death";

        private const float AttackDistance = 14f;
        private const float DelayBeforeRemove = 5f;

        private string _lastAnimationKey;

        private NavMeshAgent _agent;
        private Animator _animator;
        private Collider _collider;

        public override int MaxHealth => 150;
        public virtual int Damage => 10;

        private bool NeedAttack => Target != null && (Target.transform.position - transform.position).sqrMagnitude <= AttackDistance * AttackDistance;

        public override Team TeamId => Team.Enemy;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
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

        protected override void Die()
        {
            DropBadge();
            PlayAnimation(DeathAnimationKey);
            _collider.isTrigger = true;

            StartCoroutine(WaitAndDestroy());
        }


        protected override void OnEnemyFinded(Damageable enemy)
        {
            if (Target == null)
                Target = enemy;

            else if (enemy is Tank && Target is not Tank)
            {
                Target = enemy;
                Debug.Log("Enemy finde tank");
            }
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

        private void DropBadge()
        {
            var badge = Instantiate(_badgePrefab, transform.position, Quaternion.identity, transform.parent);
            badge.AddDropForce();
        }

        private IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(DelayBeforeRemove);
            Destroy(gameObject);
        }
    }
}
