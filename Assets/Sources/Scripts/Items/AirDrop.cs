using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class AirDrop : GameDrop
    {
        [SerializeField] private Transform _parahute;

        private const float FlyDuration = 4f;

        public override void Init(List<ItemCount> items, bool needWathRewardedVideoToGet)
        {
            Checker.gameObject.SetActive(false);
            base.Init(items, needWathRewardedVideoToGet);
        }

        public void MoveToDropPoint(Vector3 position)
        {
            transform.DOMove(position, FlyDuration).SetLink(gameObject).SetEase(Ease.Linear).OnComplete(HideParachute);
        }

        private void HideParachute()
        {
            _parahute.transform.parent = transform.parent;
            _parahute.DOScale(0, 0.4f).SetEase(Ease.Linear).SetUpdate(true).SetLink(gameObject).OnComplete(OnParachuteComplete);

            PlayFinishAnimation();

            void OnParachuteComplete()
            {
                _parahute.gameObject.SetActive(false);
                _parahute.parent = transform;
            }
        }

        private void PlayFinishAnimation()
        {
            Checker.gameObject.SetActive(true);

            Sequence sequense = DOTween.Sequence().SetLink(gameObject).SetEase(Ease.Linear);

            sequense.Append(transform.DOScale(new Vector3(1.4f, 0.75f, 1.4f), 0.25f));
            sequense.Append(transform.DOScale(new Vector3(0.8f, 1.25f, 0.8f), 0.25f));
            sequense.Append(transform.DOScale(new Vector3(1.2f, 0.8f, 1.2f), 0.15f));
            sequense.Append(transform.DOScale(new Vector3(1, 1, 1), 0.1f));

            sequense.Play();
        }
    }
}
