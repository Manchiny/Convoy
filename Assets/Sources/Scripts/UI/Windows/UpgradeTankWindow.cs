using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UpgradeTankWindow : AbstractWindow
    {
        [SerializeField] private TextMeshProUGUI _textTitle;

        private const float FadeDuration = 1f;
        private const string TapToStartKey = "tapToStart";

        public override string LockKey => "UpgradeTankWindow";

        public static UpgradeTankWindow Show() =>
                       Game.Windows.ScreenChange<UpgradeTankWindow>(true, w => w.Init());
        protected void Init()
        {
            Game.Instance.SetMode(Game.GameMode.Pause);
            SetText();
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
