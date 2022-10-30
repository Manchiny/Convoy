using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DropDawnPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _arrows;

        private const float AnimationDuration = 0.35f;

        private const float ArrowsZRotationOnHide = 0;
        private const float ArrowsZRotationOnShow = 180;

        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private Sequence _sequence;

        private bool _isActive;
        public bool IsActive => _isActive;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
            _canvasGroup = GetComponent<CanvasGroup>();
            Hide(true);
        }

        public void Show()
        {
            _isActive = true;

            SetArrowsRotation(ArrowsZRotationOnShow);

            _sequence?.Complete();
            _sequence = DOTween.Sequence()
                .Append(_rectTransform.DOScale(Vector3.one, AnimationDuration))
                .Join(_canvasGroup.DOFade(1, AnimationDuration))
                .SetLink(gameObject)
                .SetUpdate(true);
        }

        public void Hide(bool force = false)
        {
            _isActive = false;

            var scale = Vector3.one;
            scale.y = 0;

            SetArrowsRotation(ArrowsZRotationOnHide);

            _sequence?.Complete();
            _sequence = DOTween.Sequence()
                .SetLink(gameObject)
                .Append(_rectTransform.DOScale(scale, force ? 0 : AnimationDuration))
                .Join(_canvasGroup.DOFade(0, AnimationDuration))
                .SetUpdate(true);
        }

        private void SetArrowsRotation(float rotationZ)
        {
            var rotation = _arrows.rotation.eulerAngles;
            rotation.z = rotationZ;

            _arrows.rotation = Quaternion.Euler(rotation);
        }
    }
}
