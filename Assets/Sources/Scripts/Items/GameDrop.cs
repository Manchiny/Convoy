using Assets.Scripts.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class GameDrop : MonoBehaviour
    {
        [SerializeField] private GameDropChecker _playerChecker;

        private List<ItemCount> _items;
        private bool _needWathRewardedVideoToGet;

        private AbstractWindow _window;

        private void Start()
        {
            _playerChecker.PlayerEntered += OnPlyareEnter;
        }

        private void OnDisable()
        {
            _playerChecker.PlayerEntered -= OnPlyareEnter;

            if (_window != null)
                _window.Closed -= OnWindowClosed;
        }

        public void Init(List<ItemCount> items, bool needWathRewardedVideoToGet)
        {
            _items = items;
            _needWathRewardedVideoToGet = needWathRewardedVideoToGet;
        }

        private void OnPlyareEnter()
        {
            if (_needWathRewardedVideoToGet)
            {
                _window = WatchAdsRewardWindow.Show(_items, OnRewardGeted);
                _window.Closed += OnWindowClosed;
            }
            else
            {
                // TODO: обычное окно награды или сразу UI дроп;
            }
        }

        private void OnRewardGeted()
        {
            _playerChecker.gameObject.SetActive(false);

            Sequence animation = DOTween.Sequence().SetLink(gameObject).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));

            animation.Append(transform.DOScale(1.5f, 0.2f));
            animation.Append(transform.DOScale(0f, 0.2f));

            animation.Play();
        }

        private void OnWindowClosed(AbstractWindow window)
        {
            if (_window != null)
            {
                _window.Closed -= OnWindowClosed;
                _window = null;
            }

            Game.Instance.SetMode(Game.GameMode.Game);
        }
    }
}
