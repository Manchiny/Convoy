using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class ShopItem 
    {
        public readonly ShopItemName Name;
        public readonly ItemCount[] Items;
        public readonly int Cost;
        public readonly MoneyTypes MoneyType;

        public ShopItem(ShopItemName name, int cost, MoneyTypes moneyType, ItemCount[] items)
        {
            Items = items;
            Cost = cost;
            MoneyType = moneyType;
            Name = name;
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
    }
}
