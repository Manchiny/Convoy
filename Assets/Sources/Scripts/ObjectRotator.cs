using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectRotator : MonoBehaviour
    {
        [SerializeField] private float RotationDuration = 3f;

        private void Start()
        {
            if(gameObject != null)
                transform.DORotate(new Vector3(0, 360, 0), RotationDuration, RotateMode.FastBeyond360)
                            .SetRelative(true)
                            .SetEase(Ease.Linear)
                            .SetLink(gameObject)
                            .SetLoops(-1, LoopType.Restart);
        }
    }
}
