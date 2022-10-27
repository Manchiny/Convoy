using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(Animator))]
    public abstract class Enemy : Unit
    {
        [SerializeField] private SolderBadge _badgePrefab;

        private const float DelayBeforeRemove = 5f;

        private Animator _animator;
        private Coroutine _dyingProcess;

        public override Team TeamId => Team.Enemy;

        public enum EnemyType
        {
            Movable,
            Tower,
            RoadShelter
        }
        
        protected EnemyAnimations Animations { get; private set; }
       
        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            Animations = new EnemyAnimations(_animator);
        }

        private void OnEnable()
        {
            Animations.Reset();
        }

        public override void OnRestart()
        {
            base.OnRestart();

            if (_dyingProcess != null)
            {
                StopCoroutine(_dyingProcess);
                _dyingProcess = null;
            }
        }

        protected sealed override void Die()
        {
            DropBadge();
            Animations.PlayAnimation(EnemyAnimations.DeathAnimationKey);

            OnDie();
           _dyingProcess = StartCoroutine(WaitAndDisable());
        }

        protected abstract void OnDie();

        protected override void OnEnenmyMissed(Damageable enemy)
        {
            if (enemy == Target || (Target != null && Target.IsAlive == false))
                Target = TryGetAnyNewTarget();
        }

        protected override void OnEnemyFinded(Damageable enemy)
        {
            if (Target == null)
                Target = enemy;

            else if (enemy is Tank && Target is not Tank)
                Target = enemy;
        }

        protected override void OnGetDamage()
        {
           // throw new System.NotImplementedException();
        }

        private void DropBadge()
        {
            var badge = Instantiate(_badgePrefab, transform.position, Quaternion.identity, transform.parent);
            badge.AddDropForce();
        }

        private IEnumerator WaitAndDisable()
        {
            yield return new WaitForSeconds(DelayBeforeRemove);
            gameObject.SetActive(false);
        }
    }
}
