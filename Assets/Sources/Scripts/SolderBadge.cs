using Assets.Scripts.Units;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(AudioSource))]
    public class SolderBadge : MonoBehaviour
    {
        private const float DeltaY = 0.14f;

        private AudioSource _audioSource;
        private Rigidbody _rigidbody;
        private BoxCollider _collider;

        private Sequence _animation;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            Game.Restarted += OnRestart;
        }

        private void OnDisable()
        {
            Game.Restarted -= OnRestart;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player) && player.IsAlive)
                player.TakeBadge(this);
        }

        public void AddDropForce()
        {
            _rigidbody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
            _rigidbody.AddTorque(transform.up * 20f);
            _rigidbody.AddTorque(transform.forward * 50f);
        }

        public void MoveToHolder(Transform badgeHolder, Transform bezierControlPoint1, Transform bezierControlPoint2, int positionNumber)
        {
            _rigidbody.isKinematic = true;
            _collider.isTrigger = true;

            transform.parent = badgeHolder;

            Vector3 endPosition = Vector3.zero;
            endPosition.y += DeltaY * positionNumber;

            _audioSource.Play();

             _animation = DOTween.Sequence().SetEase(Ease.Linear).SetLink(gameObject).OnComplete(() => PlayResizeAnimation(badgeHolder, positionNumber));

            _animation.Append(transform.DOLocalRotate(Vector3.zero, 0.2f));
            _animation.Play();
            _animation.Insert(0, Utils.GetBezierLocalPathAnimation(transform, bezierControlPoint1, bezierControlPoint2, endPosition, 0.25f));
        }

        public void Drop()
        {
            if (_animation != null)
                _animation.Kill();

            transform.parent = null;

            _collider.isTrigger = false;
            _rigidbody.isKinematic = false;
        }

        private void PlayResizeAnimation(Transform badgeHolder, int positionNumber)
        {
            _animation.Kill();

            _animation = DOTween.Sequence().SetEase(Ease.Linear).SetLink(gameObject);

            _animation.Append(transform.DOScale(2.15f, 0.16f));
            _animation.Append(transform.DOScale(1f, 0.08f));

            _animation.Play();
        }

        private void OnRestart()
        {
            Destroy(gameObject);
        }
    }
}
