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

        private const string LanguageLocalizationKey = "language";
        private const string TitleLocalizationKey = "settings";

        private Game.GameMode _gameMode;

        public override string LockKey => "SettingsWindow";
        public override bool AnimatedClose => true;
        public override bool NeedHideHudOnShow => true;

        public static SettingsWindow Show() =>
                       Game.Windows.ScreenChange<SettingsWindow>(false, w => w.Init());

        protected void Init()
        {
            _gameMode = Game.CurrentMode;

            Game.Instance.SetMode(Game.GameMode.Pause);

            InitSocialButton();

            SetText();

            foreach (var lang in GameLocalization.AvailableLocals)
            {
                ChangeLanguageButton button = Instantiate(_languageButtonPrefab, _languageButtonContainer);
                button.Init(lang);
            }
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
        }

        private void InitSocialButton()
        {
            if (Game.SocialAdapter != null)
            {
                if (Game.SocialAdapter.IsInited && Game.SocialAdapter.IsAuthorized == false) // || Game.SocialAdapter.IsInited && Game.SocialAdapter.HasPersonalDataPermission == false)
                {
                    _connectToSocial.gameObject.SetActive(true);
                    _connectToSocial.AddListener(() => Game.SocialAdapter.ConnectProfileToSocial(OnAuthorizationSuccess, OnAuthorizationError));

                    return;
                }
            }

            _connectToSocial.gameObject.SetActive(false);
        }

        private void OnAuthorizationSuccess()
        {
            _connectToSocial.gameObject.SetActive(false);
            Game.Instance.SetSaver(Game.SocialAdapter.GetSaver);
            Game.SocialAdapter.RequestPersonalProfileDataPermission();
        }

        private void OnAuthorizationError(string text)
        {
            _connectToSocial.gameObject.SetActive(false);
            Debug.Log($"Error autorization: {text}");
        }
    }
}
