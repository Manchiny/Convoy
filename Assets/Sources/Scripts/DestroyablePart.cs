using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Destroyable
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Restartable))]
    public class DestroyablePart : MonoBehaviour, IRestartable
    {
        private const float LiveTime = 5f;
        private const float ExplosionForce = 500f;

        private const float DisappearanceDuration = 0.2f;

        private Collider _collider;
        private bool _isTrigger;

        private Rigidbody _rigidbody;
        private Coroutine _disabling;

        private Tween _disappearanceAnimation;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            if (_collider != null)
                _isTrigger = _collider.isTrigger;
        }

        public void Crush(Vector3 explosionPosition)
        {
            _rigidbody.isKinematic = false;

            _rigidbody.AddExplosionForce(ExplosionForce, explosionPosition, 30f);
            _disabling = StartCoroutine(WaitAndDestroy());
        }

        private IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(LiveTime);

            if (_collider != null)
                _collider.isTrigger = true;

            _disappearanceAnimation = transform.DOScale(Vector3.zero, DisappearanceDuration).SetLink(gameObject).SetEase(Ease.Linear).OnComplete(() => gameObject.SetActive(false));
        }

        public void OnRestart()
        {
            if (_disabling != null)
            {
                StopCoroutine(_disabling);
                _disabling = null;
            }

            if (_disappearanceAnimation != null)
                _disappearanceAnimation.Kill();

            if (_collider != null)
                _collider.isTrigger = _isTrigger;

            _rigidbody.isKinematic = true;
        }
    }
}
