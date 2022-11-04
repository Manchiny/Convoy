using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _badgesText;
        [Space]
        [SerializeField] private RectTransform _badgesPanelContent;
        [Space]
        [SerializeField] private BasicButton _settingsButton;
        [SerializeField] private BasicButton _leaderboardButton;
        [Space]
        [SerializeField] private BoostsPanel _playerBoostsPanel;
        [SerializeField] private BoostsPanel _tankBoostsPanel;

        private const string LevelLocalizationKey = "level";

        private const float FadeDuration = 1f;
        private const float MoneyPanelAnimationDuration = 0.075f;
        private const float FloatingMoneyDeltaYStartPosition = 1f;

        private CanvasGroup _canvas;

        protected Tween _showHideAnimation;
        private Tween _badgePanelAnimationTween;

        private void Awake()
        {
            _canvas = GetComponent<CanvasGroup>();
        }

        private void OnDestroy()
        {
            Game.Player.BadgesChanged -= OnBadgesChanged;
            //Game.Localization.LanguageChanged -= OnLocalizationChanged;

            // _settingsButton.RemoveListener(OnSettingsButtonClick);
        }

        public void Init(UserData userData, bool isReinit = false)
        {
            _canvas.alpha = 0;

            _playerBoostsPanel.Init(userData);
            _tankBoostsPanel.Init(userData);

            //SetMoneyText(Game.User.Money);

            if (isReinit == false)
            {
                _settingsButton.AddListener(OnSettingsButtonClick);
                Game.Player.BadgesChanged += OnBadgesChanged;
            }
            //else
            //    OnLevelChanged(Game.Instance.CurrentLevelId.Value);

            Show();
        }

        public void Show()
        {
            if (_showHideAnimation != null && _showHideAnimation.active)
            {
                _showHideAnimation.Kill();
                _showHideAnimation = null;
            }

            _showHideAnimation = _canvas.DOFade(1f, FadeDuration).SetLink(gameObject);
        }

        public void Hide()
        {
            if (_showHideAnimation != null && _showHideAnimation.active)
            {
                _showHideAnimation.Kill();
                _showHideAnimation = null;
            }

            _showHideAnimation = _canvas.DOFade(0f, FadeDuration).SetLink(gameObject).SetUpdate(true);
        }

        //public void ShowFloatingMoney(int count, Hand hand)
        //{
        //    Vector3 position = hand.transform.position;
        //    position.y += FloatingMoneyDeltaYStartPosition;

        //    floatingMoney.transform.position = Camera.main.WorldToScreenPoint(position);
        //    floatingMoney.Init(count);
        //}

        private void OnBadgesChanged(int badgesCount)
        {
            SetBadgesText(badgesCount);
            PlayBadgesPanelAnimation();
        }

        private void SetBadgesText(int badgesCount)
        {
            _badgesText.text = badgesCount.ToString();
        }

        private void PlayBadgesPanelAnimation()
        {
            if (_badgePanelAnimationTween != null)
                _badgePanelAnimationTween.Kill();

            var sequence = DOTween.Sequence().SetEase(Ease.Linear).SetLink(gameObject);

            sequence.Append(_badgesPanelContent.DOScale(1.5f, MoneyPanelAnimationDuration));
            sequence.Append(_badgesPanelContent.DOScale(1f, MoneyPanelAnimationDuration));

            _badgePanelAnimationTween = sequence;

            _badgePanelAnimationTween.Play();
        }

        private void OnSettingsButtonClick()
        {
            SettingsWindow.Show();
        }
    }
}
