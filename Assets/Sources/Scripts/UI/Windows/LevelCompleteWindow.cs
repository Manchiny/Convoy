using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class LevelCompleteWindow : AbstractWindow
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _youGotText;
        [SerializeField] private TextMeshProUGUI _badgesCountText;
        [Space]
        [SerializeField] private BasicButton _continueButton;
        [SerializeField] private BasicButton _doubleRewardButton;
        [SerializeField] private TextMeshProUGUI _adsText;

        private const string TitleLocalizationKey = "level_complete";
        private const string ContinueLocalizationKey = "continue";
        private const string YouRewardLocalizationKey = "you_reward";
        private const string DoubleLoacalizationKey = "double";
        private const string AdsLocalizationKey = "ad";

        private int _levelId;

        private int _badgesCount;
        private int _totelBadges;

        private event Action _onContinueCallback;

        public override string LockKey => "LevelCompleteWindow";
        public override bool AnimatedClose => true;

        public static LevelCompleteWindow Show(int levelId, int badgesCount, Action onContinue) =>
                       Game.Windows.ScreenChange<LevelCompleteWindow>(true, w => w.Init(levelId, badgesCount, onContinue));

        protected override void SetText()
        {
            _titleText.text = TitleLocalizationKey.Localize((_levelId + 1).ToString());
            _youGotText.text = YouRewardLocalizationKey.Localize();
            _badgesCountText.text = _totelBadges.ToString();

            _continueButton.Text = ContinueLocalizationKey.Localize();
            _doubleRewardButton.Text = DoubleLoacalizationKey.Localize();
            _adsText.text = AdsLocalizationKey.Localize();
        }

        private void Init(int levelId, int badgesCount, Action onContinue)
        {
            _levelId = levelId;
            _badgesCount = badgesCount;
            _totelBadges = badgesCount;

            _onContinueCallback = onContinue;

            SetText();

            _continueButton.SetOnClick(OnContinueButtonClick);
            _doubleRewardButton.SetOnClick(OnDoubleRewardButtonClick);
        }

        private void OnContinueButtonClick()
        {
            Game.Instance.AddBadges(_badgesCount);

            Close();

            if (_onContinueCallback != null)
                _onContinueCallback?.Invoke();
        }

        private void OnDoubleRewardButtonClick()
        {
            bool rewarded = false;
            Game.Adverts.TryShowRewardedVideo(OnAdOpened, () => rewarded = true, OnAdsClosed, null);

            void OnAdOpened()
            {
                _doubleRewardButton.gameObject.SetActive(false);
            }

            void OnAdsClosed()
            {
                if(rewarded)
                {
                    _totelBadges = _badgesCount * 2;
                    _badgesCountText.text = _totelBadges.ToString();
                }
            }
        }
    }
}
