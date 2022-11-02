using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class ShopItem 
    {
        [SerializeField] private bool _showInShop = false;
        [SerializeField] private ShopItemName _name;
        [SerializeField] private MoneyTypes _moneyType;
        [SerializeField] private int _cost;
        [SerializeField] private Sprite _icon;
        [SerializeField] private List<ItemCount> _items;

        public ShopItemName Name => _name;
        public MoneyTypes MoneyType => _moneyType;
        public int Cost => _cost;
        public IReadOnlyList<ItemCount> Items => _items;
        public Sprite Icon => _icon;
        public bool ShowInShop => _showInShop;

        public enum MoneyTypes
        {
            Soft,
            Ads
        }
    }

    public enum ShopItemName
    {
        PlayerMaxHealthPropertyPointAds,
        PlayerArmorPropertyPointAds,
        PlayerDamagePropertyPointAds,
        PlyaerShootingSpeedPropertyAds,

        TankMaxHealthPropertyPointAds,
        TankArmorPropertyPointAds,
        TankDamagePropertyPointAds,
        TankShootingSpeedPropertyAds,

        PlayerMaxHealthPropertyPointSoft,
        PlayerArmorPropertyPointSoft,
        PlayerDamagePropertyPointSoft,
        PlyaerShootingSpeedPropertySoft,

        PlayerDoubleArmoreBoostSoft,
        PlayerDobleDamageBoostSoft,
        PlayerDounleShootingSpeedSoft,

        TankDoubleArmorBoostSoft,
        TankDobleDamageBoostSoft,
        TankDounleShootingSpeedSoft,

        PlayerHeal20Soft,
        TankHeal20Soft,

        BadgesAds,
    }
}
