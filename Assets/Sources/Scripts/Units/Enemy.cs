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

        public override Team TeamId => Team.Enemy;
        public override int MaxHealth => 150;

        public enum EnemyType
        {
            Movable,
            Tower
        }

        protected EnemyAnimations Animations { get; private set; }

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            Animations = new EnemyAnimations(_animator);
        }

        protected sealed override void Die()
        {
            DropBadge();
            Animations.PlayAnimation(EnemyAnimations.DeathAnimationKey);

            OnDie();
            StartCoroutine(WaitAndDisable());
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
            {
                Target = enemy;
                Debug.Log("Enemy finde tank");
            }
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
