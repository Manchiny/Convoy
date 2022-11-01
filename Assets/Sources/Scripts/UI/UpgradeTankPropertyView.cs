using Assets.Scripts.Items;
using System.Linq;
using TMPro;
using UnityEngine;
using static Assets.Scripts.UnitPropertyLevels;

namespace Assets.Scripts.UI
{
    public class UpgradeTankPropertyView : ShopItemView
    {
        [SerializeField] private PropertyProgressPanel _progresPanel;
        [SerializeField] private TextMeshProUGUI _levelsCountText;

        private UnitPropertyType _propertyType;
        private Item _item;

        private UnitData _data;

        private int _maxPropertyLevel;
        private int _currentPropertyLevel;
        private int _currentPropertyPoints;

        public override void Init()
        {
            base.Init();

            _item = ShopItem.Items.First().Item;
            _propertyType = Item.UnitPropertyByItemType(_item.Type);
            _data = Game.Tank.GetData;

            UpdateView();
        }

        protected override void OnSuccesBuy()
        {
            Game.Instance.TryUseItem(_item, null, OnUse);

            void OnUse()
            {
                base.OnSuccesBuy();
                UpdateView();
            }
        }

        private void UpdateView()
        {
            _maxPropertyLevel = _data.MaxPropertyLevel(_propertyType, Game.Tank.PropertiesDatabase);
            _currentPropertyLevel = _data.GetUserPropertyLevel(_propertyType);
            _currentPropertyPoints = _data.CurrentPropertyPoints(_propertyType);

            _levelsCountText.text = $"{_currentPropertyLevel}/{_maxPropertyLevel}";


            if (_currentPropertyLevel == _maxPropertyLevel)
            {
                _progresPanel.SetMax();
                BuyButton.SetLock(true);
            }
            else
                _progresPanel.SetProgress(_currentPropertyPoints);
        }
    }
}
