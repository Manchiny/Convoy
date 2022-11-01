using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class ShopItem 
    {
        [SerializeField] private ShopItemName _name;
        [SerializeField] private MoneyTypes _moneyType;
        [SerializeField] private int _cost;
        [SerializeField] private List<ItemCount> _items;

        public ShopItemName Name => _name;
        public MoneyTypes MoneyType => _moneyType;
        public int Cost => _cost;
        public IReadOnlyList<ItemCount> Items => _items;

        public ShopItem(ShopItemName name, int cost, MoneyTypes moneyType, List<ItemCount> items)
        {
            _name = name;
            _moneyType = moneyType;
            _cost = cost;
            _items = items;
        }

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
    }
}
