using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class Enemy : Character
    {
        [SerializeField] private Transform _target;
      //  [SerializeField] private HandAttackChecker _attackChecker;

        private const string IdleAnimationKey = "Idle";
        private const string RunAnimationKey = "Run";
        private const string AttackAnimationKey = "Attack";

        private const float DestinationDistance = 1f;

        private string _lastAnimationKey;

        private NavMeshAgent _agent;
        private Animator _animator;

        private HashSet<Damageable> _attackTargets = new();
        public override int MaxHealth => 30;
        public virtual int Damage => 10;

        private bool NeedAttack => _attackTargets.Count > 0;
      //  private bool CanAttackPlayer => (Game.Player.transform.position - transform.position).sqrMagnitude <= DestinationDistance * DestinationDistance;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            DisableAttackCheker();
        }

        private void Update()
        {
            //if (CanAttackPlayer)
            //    Attack(Game.Player);
            //else if (NeedAttack)
            //    Attack(_attackTargets.First());
            //else if (_target != null)
            //    MoveTo(_target);
            //else
            //    StopAnithing();
        }

        public void OnAttackableEnter(IEnemyTarget damageable)
        {
            if (damageable.Damageable.IsAlive)
                _attackTargets.Add(damageable.Damageable);
        }

        public void OnAttackableExit(IEnemyTarget damageable)
        {
            _attackTargets.Remove(damageable.Damageable);
        }

        protected override void OnGetDamage()
        {
            //  throw new System.NotImplementedException();
        }

        private void Attack(Damageable damageable)
        {
            if (damageable.IsAlive == false)
                _attackTargets.Remove(damageable);
            else
            {
                //if ((damageable.transform.position - transform.position).sqrMagnitude < DestinationDistance * DestinationDistance)
                //{
                _agent.ResetPath();

                transform.LookAt(damageable.transform);
                PlayAnimation(AttackAnimationKey);
                //}
                //else
                //    MoveTo(damageable.transform);
            }
        }

        private void MoveTo(Transform target)
        {
            DisableAttackCheker();

            PlayAnimation(RunAnimationKey);

            //if (CanAttackPlayer == false)
            //    _agent.SetDestination(target.position);
        }

        private void StopAnithing()
        {
            DisableAttackCheker();
            PlayAnimation(IdleAnimationKey);
        }

        private void PlayAnimation(string animationKey)
        {
            if (_lastAnimationKey == animationKey)
                return;

            _lastAnimationKey = animationKey;

            _animator.StopPlayback();
            _animator.CrossFade(animationKey, 0.15f);
        }

        protected override void Die()
        {
            gameObject.SetActive(false);
        }

        protected void EnableAttackChecker()
        {
          //  _attackChecker.gameObject.SetActive(true);
        }

        public void DisableAttackCheker()
        {
            //_attackChecker.gameObject.SetActive(false);
        }
    }
}
