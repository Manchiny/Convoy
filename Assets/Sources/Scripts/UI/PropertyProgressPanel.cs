using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class PropertyProgressPanel : MonoBehaviour
    {
        [SerializeField] private Color _baseIndicatorColor;
        [SerializeField] private Color _onUpgradedIndicatorColor;
        [SerializeField] private List<PropertyPointIndicator> _indicators;

        private Sequence _animation;

        private void Awake()
        {
            foreach (var item in _indicators)
            {
                item.Init(_baseIndicatorColor, _onUpgradedIndicatorColor);
            }
        }

        public void SetProgress(int currentPropertyPoints, bool needAnimate)
        {
            int counter = 0;

            for (int i = 0; i < currentPropertyPoints; i++)
            {
                _indicators[i].SetUpgraded();
                counter++;
            }

            if(counter < _indicators.Count)
            {
                for (int i = counter; i < _indicators.Count; i++)
                {
                    _indicators[i].Reset();
                }
            }

            if (needAnimate)
                ShowAnimation();
        }

        public void SetMax(bool needAnimate)
        {
            foreach (var indicator in _indicators)
                indicator.SetUpgraded();

            if (needAnimate)
                ShowAnimation();
        }

        private void ShowAnimation()
        {
            if (_animation != null)
                _animation.Kill();

            _animation = DOTween.Sequence().SetLink(gameObject).SetEase(Ease.Linear).SetUpdate(true);

            _animation.Append(transform.DOScale(1.5f, 0.05f));
            _animation.Append(transform.DOScale(0.6f, 0.05f));
            _animation.Append(transform.DOScale(1.2f, 0.04f));
            _animation.Append(transform.DOScale(0.8f, 0.04f));
            _animation.Append(transform.DOScale(1f, 0.025f));
        }
    }
}
