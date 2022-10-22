using Assets.Scripts.Units;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class SolderBadge : MonoBehaviour
    {
        private const float DeltaY = 0.18f;

        private Rigidbody _rigidbody;
        private BoxCollider _collider;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<BoxCollider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
                player.TakeBadge(this);
        }

        public void AddDropForce()
        {
            _rigidbody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
            _rigidbody.AddTorque(transform.up * 20f);
            _rigidbody.AddTorque(transform.forward * 50f);
        }

        public void MoveToHolder(Transform badgeHolder, int positionNumber)
        {
            _rigidbody.isKinematic = true;
            _collider.isTrigger = true;

            Vector3 position = badgeHolder.transform.position;
            position.y += DeltaY * positionNumber;

            Sequence sequence = DOTween.Sequence().SetEase(Ease.Linear).SetLink(gameObject).OnComplete(() => PlayResizeAnimation(badgeHolder, positionNumber));

            sequence.Insert(0, transform.DOMove(position, 0.3f));
            sequence.Insert(0, transform.DORotate(badgeHolder.transform.rotation.eulerAngles, 0.3f));

        }

        private void PlayResizeAnimation(Transform badgeHolder, int positionNumber)
        {
            transform.parent = badgeHolder;

            Vector3 position = Vector3.zero;
            position.y = DeltaY * positionNumber;
            transform.localPosition = position;

            Sequence sequence = DOTween.Sequence().SetEase(Ease.Linear).SetLink(gameObject);

            sequence.Append(transform.DOLocalRotate(Vector3.zero, 0.1f));
            sequence.Append(transform.DOScale(2f, 0.2f));
            sequence.Append(transform.DOScale(1f, 0.1f));
        }
    }
}
