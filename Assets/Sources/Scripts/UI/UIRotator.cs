using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public class UIRotator : MonoBehaviour
    {
        [SerializeField] private float RotationDuration = 3f;

        private Tween _animation;

        private void OnEnable()
        {
            if (gameObject != null)
                _animation = transform.DOLocalRotate(new Vector3(0, 0, -360), RotationDuration, RotateMode.FastBeyond360)
                                    .SetRelative(true)
                                    .SetEase(Ease.Linear)
                                    .SetLink(gameObject)
                                    .SetLoops(-1, LoopType.Restart)
                                    .SetUpdate(true);
        }

        private void OnDisable()
        {
            if (_animation != null)
                _animation.Kill();
        }
    }
}
