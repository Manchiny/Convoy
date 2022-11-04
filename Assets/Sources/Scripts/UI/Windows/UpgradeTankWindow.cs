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
        
        private List<UpgradeUnitPropertyView> _propertyViews;

        public override string LockKey => "UpgradeTankWindow";
        public override bool AnimatedClose => true;

        public static UpgradeTankWindow Show() =>
                       Game.Windows.ScreenChange<UpgradeTankWindow>(false, w => w.Init());

        protected override void OnAwake()
        {
            base.OnAwake();
            _propertyViews = GetComponentsInChildren<UpgradeUnitPropertyView>().ToList();
        }

        protected void Init()
        {
            Game.Instance.SetMode(Game.GameMode.PuaseTankView);
            SetText();

            foreach (var view in _propertyViews)
            {
                view.Init(null, Game.Tank);
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
