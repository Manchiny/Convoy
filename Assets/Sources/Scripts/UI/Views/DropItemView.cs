using Assets.Scripts.Items;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DropItemView : ItemView
    {
        private const float DropAnimationDuration = 1.1f;
        private const float UnhideDuration = 0.1f;
        private const float HideDuration = 0.9f;

        private Sequence _dropAnimations;
        private CanvasGroup _canvas;


        private void Awake()
        {
            _canvas = GetComponent<CanvasGroup>();
        }

        public override void Init(ItemCount itemCount)
        {
            base.Init(itemCount);
            _canvas.alpha = 0;
        }

        public void Show()
        {
            _dropAnimations = DOTween.Sequence().SetLink(gameObject).SetEase(Ease.InCubic).SetUpdate(true).OnComplete(() => Destroy(gameObject));

            _dropAnimations.Append(_canvas.DOFade(1, UnhideDuration));
            _dropAnimations.Append(_canvas.DOFade(0, HideDuration));
            _dropAnimations.Play();
            _dropAnimations.Insert(0, MoveUp());
        }

        private Tween MoveUp()
        {
            var rect = transform as RectTransform;
            return rect.DOMove(new Vector3(rect.position.x, Screen.height, 0f), DropAnimationDuration);
        }
    }
}
