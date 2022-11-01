using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UpgradePlayerWindow : AbstractWindow
    {
        [SerializeField] private TextMeshProUGUI _textTitle;

        public const string UpgradePlayerLocalizationKey = "upgrade_player";

        private List<UpgradeUnitPropertyView> _propertyViews;

        public override string LockKey => "UpgradePlayerWindow";

        public static UpgradePlayerWindow Show() =>
                       Game.Windows.ScreenChange<UpgradePlayerWindow>(false, w => w.Init());

        protected override void OnAwake()
        {
            base.OnAwake();
            _propertyViews = GetComponentsInChildren<UpgradeUnitPropertyView>().ToList();
        }

        protected void Init()
        {
            SetText();

            foreach (var view in _propertyViews)
            {
                view.Init(Game.Player);
            }
        }

        protected override void SetText()
        {
            _textTitle.text = UpgradePlayerLocalizationKey.Localize();
        }
    }
}
