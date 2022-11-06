using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public const float MoveDistance = 100f;

        private const float MoveAnimationDuration = 2f;
        private const float UnhideDuration = 0.1f;
        private const float HideDuration = 3f;

        private CanvasGroup _canvas;

        private void Awake()
        {
            _canvas = GetComponent<CanvasGroup>();
            _canvas.alpha = 0;
        }

        public void Show(string text)
        {
            _text.text = text;

            RectTransform rect = transform as RectTransform;

            if (Screen.height - rect.position.y < MoveDistance * 3f)
                rect.position = new Vector2(rect.position.x, Screen.height/2f);

            PlayAnimation();
        }

        private void PlayAnimation()
        {
            Sequence sequence = DOTween.Sequence().SetLink(gameObject).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => Destroy(gameObject));

            sequence.Append(_canvas.DOFade(1, UnhideDuration));
            sequence.Append(_canvas.DOFade(0, HideDuration));
            sequence.Play();
            sequence.Insert(0, MoveUp());
        }

        private Tween MoveUp()
        {
            var rect = transform as RectTransform;
            return rect.DOMove(new Vector3(rect.position.x, rect.position.y + MoveDistance, 0f), MoveAnimationDuration);
        }
    }
}