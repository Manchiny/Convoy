using Assets.Scripts.Localization;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SettingsWindow : AbstractWindow
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [Space]
        [SerializeField] private TextMeshProUGUI _languageSettingsText;
        [SerializeField] private RectTransform _languageButtonContainer;
        [SerializeField] private ChangeLanguageButton _languageButtonPrefab;
        [Space]
        [SerializeField] private BasicButton _connectToSocial;
        [SerializeField] private BasicButton _leaderBoardButton;
        [Space]
        [SerializeField] private SliderParametrPanel _soundPanlel;

        private const string LanguageLocalizationKey = "language";
        private const string TitleLocalizationKey = "settings";
        private const string LeaderboardLocalizationKey = "leaderboard";
        private const string SoundLocalizationKey = "sound";

        private Game.GameMode _gameMode;

        public override string LockKey => "SettingsWindow";
        public override bool AnimatedClose => true;
        public override bool NeedHideHudOnShow => true;

        private AbstractWindow _leaderboard;

        public static SettingsWindow Show() =>
                       Game.Windows.ScreenChange<SettingsWindow>(false, w => w.Init());

        private void OnDestroy()
        {
            if (_leaderboard != null)
                _leaderboard.Closed -= OnLeaderboardClose;
        }

        protected void Init()
        {
            _gameMode = Game.CurrentMode;

            Game.Instance.SetMode(Game.GameMode.Pause);

            InitSocialButton();

            _leaderBoardButton.gameObject.SetActive(Game.SocialAdapter != null && Game.SocialAdapter.IsInited && Game.SocialAdapter.IsAuthorized);
            _leaderBoardButton.AddListener(OnLeaderboardButtonClick);

            SetText();

            foreach (var lang in GameLocalization.AvailableLocals)
            {
                ChangeLanguageButton button = Instantiate(_languageButtonPrefab, _languageButtonContainer);
                button.Init(lang);
            }

            _soundPanlel.Init(Game.Sound.Enabled, SoundLocalizationKey.Localize(), OnSoundSliderClicked);
        }

        protected override void OnClose()
        {
            Game.Instance.SetMode(_gameMode);
        }

        protected override void SetText()
        {
            _languageSettingsText.text = LanguageLocalizationKey.Localize() + ":";
            _titleText.text = TitleLocalizationKey.Localize();

            if (_connectToSocial.gameObject.activeInHierarchy)
                _connectToSocial.Text = "connect_to".Localize() + $" {Game.SocialAdapter.Name}";

            if (_leaderBoardButton.gameObject.activeInHierarchy)
                _leaderBoardButton.Text = LeaderboardLocalizationKey.Localize();

            _soundPanlel.SetText(SoundLocalizationKey);
        }

        private void OnSoundSliderClicked(bool enabled)
        {
            Game.Instance.SetSound(enabled);
        }   

        private void InitSocialButton()
        {
            if (Game.SocialAdapter != null && Game.SocialAdapter.IsInited)
            {
                _connectToSocial.gameObject.SetActive(true);

                if (Game.SocialAdapter.IsAuthorized == false)       
                    _connectToSocial.SetOnClick(() => Game.SocialAdapter.ConnectProfileToSocial(OnAuthorizationSuccess, OnAuthorizationError));
                else
                    _connectToSocial.gameObject.SetActive(false);
            }
            else
                _connectToSocial.gameObject.SetActive(false);
        }

        private void OnAuthorizationSuccess()
        {
            Game.Instance.SetSaver(Game.SocialAdapter.GetSaver);
            Game.SocialAdapter.RequestPersonalProfileDataPermission(OnGetDataPermissionSuccess, OnAuthorizationError);

            _leaderBoardButton.gameObject.SetActive(Game.SocialAdapter != null && Game.SocialAdapter.IsInited && Game.SocialAdapter.IsAuthorized);
        }

        private void OnGetDataPermissionSuccess()
        {
            _connectToSocial.gameObject.SetActive(false);
        }

        private void OnAuthorizationError(string text)
        {
            _connectToSocial.gameObject.SetActive(false);
            Debug.Log($"Error autorization: {text}");
        }

        private void OnLeaderboardButtonClick()
        {
            if (Game.SocialAdapter != null && Game.SocialAdapter.IsAuthorized)
            {
                _leaderBoardButton.SetLock(true);

                _leaderboard = LeaderboardWindow.Show();
                _leaderboard.Closed += OnLeaderboardClose;
            }
        }

        private void OnLeaderboardClose(AbstractWindow window)
        {
            _leaderBoardButton.SetLock(false);
        }
    }
}
