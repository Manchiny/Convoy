using UnityEngine;

namespace Assets.Scripts.Units
{
    public class EnemyAnimations 
    {
        public const string IdleAnimationKey = "Idle";
        public const string RunAnimationKey = "Run";
        public const string AttackAnimationKey = "Idle";
        public const string DeathAnimationKey = "Death";

        private Animator _animator;
        private string _lastAnimationKey;

        public EnemyAnimations(Animator animator)
        {
            _animator = animator;
        }

        public void PlayAnimation(string animationKey)
        {
            if (_lastAnimationKey == animationKey)
                return;

            _lastAnimationKey = animationKey;

            _animator.StopPlayback();
            _animator.CrossFade(animationKey, 0.15f);
        }

        public void Reset()
        {
            _lastAnimationKey = "";
            PlayAnimation(IdleAnimationKey);
        }
    }
}
