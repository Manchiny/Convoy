using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UpgradeTankWindow : AbstractWindow
    {
        [SerializeField] private TextMeshProUGUI _textTitle;

        private const float FadeDuration = 1f;
        public override string LockKey => "UpgradeTankWindow";

        public static UpgradeTankWindow Show() =>
                       Game.Windows.ScreenChange<UpgradeTankWindow>(true, w => w.Init());
        protected void Init()
        {
            Game.Instance.SetMode(Game.GameMode.TankUpgrade);
            SetText();

            //if (Game.Adverts != null)
            //    Game.Adverts.TryShowInterstitial(Close);
        }

        protected override void OnClose()
        {
            Game.Instance.SetMode(Game.GameMode.Game);
        }

        protected override void SetText()
        {
            //_infoText.text = TapToStartKey.Localize();
        }
    }
}
