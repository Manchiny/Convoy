using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyPointer : MonoBehaviour
    {
        private const float EnemySizeFactor = 1.3f;
        private const float MaxScaleOffset = 1.4f;
        private const float MaxScaleFactor = 3f;

        private const float HideDuration = 0.25f;

        private Damageable _enemy;
        private Sequence _animationSequence;

        private void Start()
        {
            Game.Loosed += ForceHide;
        }

        private void OnDisable()
        {
            if(_enemy != null)
                _enemy.Died -= OnenemyDied;

            Game.Loosed -= ForceHide;
        }

        public void SetEnemy(Damageable enemy)
        {
            if(enemy == null)
                Hide();
            else if(enemy.IsAlive)
                Show(enemy);
        }

        public void ForceHide()
        {
            transform.parent = null;

            if (_animationSequence != null)
                _animationSequence.Kill();

            gameObject.SetActive(false);
        }

        private void OnenemyDied(Damageable enemy)
        {
            ForceHide();
        }

        private void Show(Damageable enemy)
        {
            if (_animationSequence != null)
                _animationSequence.Kill();

            _enemy = enemy;
            _enemy.Died += OnenemyDied;

            transform.parent = _enemy.transform;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.zero;

            Vector3 enemySize = enemy.Collider.bounds.size;
            float size = Mathf.Max(enemySize.x, enemySize.z);
            float scaleFactor = size / EnemySizeFactor;

            if (scaleFactor > MaxScaleFactor)
                scaleFactor = MaxScaleFactor;

            _animationSequence = DOTween.Sequence().SetLink(gameObject).SetEase(Ease.Linear);

            _animationSequence.Append(transform.DOScale(Vector3.one * scaleFactor * MaxScaleOffset, 0.25f));
            _animationSequence.Append(transform.DOScale(Vector3.one * scaleFactor, 0.06f));

            gameObject.SetActive(true);
            _animationSequence.Play();
        }

        private void Hide()
        {
            transform.parent = null;

            if (_animationSequence != null)
                _animationSequence.Kill();

            _animationSequence = DOTween.Sequence().SetLink(gameObject).SetEase(Ease.Linear).OnComplete(() => gameObject.SetActive(false));

            _animationSequence.Append(transform.DOScale(0, HideDuration));
            _animationSequence.Play();
        }
    }
}
