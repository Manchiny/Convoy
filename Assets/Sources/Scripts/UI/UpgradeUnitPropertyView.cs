using Assets.Scripts.Items;
using Assets.Scripts.Units;
using System.Linq;
using TMPro;
using UnityEngine;
using static Assets.Scripts.UnitPropertyLevels;

namespace Assets.Scripts.UI
{
    public class UpgradeUnitPropertyView : ShopItemView
    {
        [SerializeField] private ShopItemName _shopItemName;
        [SerializeField] private PropertyProgressPanel _progresPanel;
        [SerializeField] private TextMeshProUGUI _levelsCountText;

        private UnitPropertyType _propertyType;
        private Item _item;

        private UnitData _data;

        private int _maxPropertyLevel;
        private int _currentPropertyLevel;
        private int _currentPropertyPoints;

        private void OnDestroy()
        {
            _data.Changed -= OnDataChanged;
        }

        public override void Init(ShopItem shopItem, Unit unit)
        {
            ShopItem = Game.Shop.ShopItemsDatabase.GetShopItemByName(_shopItemName);

            base.Init(null, unit);

            _item = ShopItem.Items.First().Item;
            _propertyType = Item.UnitPropertyByItemType(_item.Type);

            _data = Unit.Data;
            _data.Changed += OnDataChanged;

            UpdateView(false);
        }

        private void OnDataChanged()
        {
            UpdateView(true);
        }

        private void UpdateView(bool needAnimateIndicatorPanel)
        {
            _maxPropertyLevel = _data.MaxPropertyLevel(_propertyType, Unit.PropertiesDatabase);

            int lastPropertyLevel = _currentPropertyLevel;
            int lastPropertyPoints = _currentPropertyPoints;

            _currentPropertyLevel = _data.GetUserPropertyLevel(_propertyType);
            _currentPropertyPoints = _data.CurrentPropertyPoints(_propertyType);

            bool dataChanged = lastPropertyLevel != _currentPropertyLevel || lastPropertyPoints != _currentPropertyPoints;

            _levelsCountText.text = $"{_currentPropertyLevel}/{_maxPropertyLevel}";

            if (_currentPropertyLevel == _maxPropertyLevel)
            {
                _progresPanel.SetMax(needAnimateIndicatorPanel);
                BuyButton.SetLock(true);
            }
            else
                _progresPanel.SetProgress(_currentPropertyPoints, needAnimateIndicatorPanel && dataChanged);
        }
    }
}
