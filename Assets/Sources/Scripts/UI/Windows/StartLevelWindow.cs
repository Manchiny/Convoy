using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class StartLevelWindow : AbstractWindow
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _badgesCountText;
        [Space]
        [SerializeField] private BasicButton _startLevelButton;
        [SerializeField] private BasicButton _upgradePlayerButton;
        [SerializeField] private BasicButton _shopButton;

        private const string LevelLocalizationKey = "level";
        private const string PlayLocalizationKey = "play";
        private const string UgradePlayerLocalizationKey = "upgrade_player";
        private const string ShopLocalizationKey = "shop";

        private event Action _onStartCallback;

        public override string LockKey => "StartLevelWindow";
        public override bool AnimatedClose => true;
        public override bool NeedHideHudOnShow => true;

        public static StartLevelWindow Show(Action onStartClick) =>
                      Game.Windows.ScreenChange<StartLevelWindow>(true, w => w.Init(onStartClick));

        private void OnDestroy()
        {
            Game.User.BadgesChaged -= ChangeBadgesText;
        }

        protected override void SetText()
        {
            _levelText.text = LevelLocalizationKey.Localize() + $" {Game.Instance.CurrentLevelId + 1}";

            _startLevelButton.Text = PlayLocalizationKey.Localize();
            _upgradePlayerButton.Text = UgradePlayerLocalizationKey.Localize();
            _shopButton.Text = ShopLocalizationKey.Localize();
        }

        private void Init(Action onStartClick)
        {
            _onStartCallback = onStartClick;

            ChangeBadgesText(Game.User.Badges);
            Game.User.BadgesChaged += ChangeBadgesText;

            _startLevelButton.SetOnClick(OnStartLevelButtonClick);
            _upgradePlayerButton.SetOnClick(OnUpgradePlayerButtonClick);
            _shopButton.SetOnClick(OnShopButtonClick);

            SetText();
        }

        private void OnStartLevelButtonClick()
        {
            _onStartCallback?.Invoke();
            Close();
        }

        private void OnShopButtonClick()
        {
            ShopWindow.Show();
        }

        private void OnUpgradePlayerButtonClick()
        {
            UpgradePlayerWindow.Show();
        }

        private void ChangeBadgesText(int count)
        {
            _badgesCountText.text = count.ToString();
        }
    }
}
