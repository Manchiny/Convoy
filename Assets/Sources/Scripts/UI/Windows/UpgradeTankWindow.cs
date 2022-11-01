using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UpgradeTankWindow : AbstractWindow
    {
        [SerializeField] private TextMeshProUGUI _textTitle;

        public const string UpgradeTankLocalizationKey = "upgrade_tank";
        private const float FadeDuration = 1f;
        
        private List<UpgradeTankPropertyView> _propertyViews;

        public override string LockKey => "UpgradeTankWindow";

        public static UpgradeTankWindow Show() =>
                       Game.Windows.ScreenChange<UpgradeTankWindow>(true, w => w.Init());

        protected override void OnAwake()
        {
            base.OnAwake();
            _propertyViews = GetComponentsInChildren<UpgradeTankPropertyView>().ToList();
        }

        protected void Init()
        {
            Game.Instance.SetMode(Game.GameMode.Puase);
            SetText();

            foreach (var view in _propertyViews)
            {
                view.Init();
            }
        }

        protected override void OnClose()
        {
            Game.Instance.SetMode(Game.GameMode.Game);
        }

        protected override void SetText()
        {
            _textTitle.text = UpgradeTankLocalizationKey.Localize();
        }
    }
}
