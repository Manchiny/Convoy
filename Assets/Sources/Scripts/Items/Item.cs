using System;
using static Assets.Scripts.Items.Item;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class Item
    {
        public ItemName Name;

        public readonly float Value;
        public readonly float Seconds;

        public bool IsTemporary => Seconds > 0;

        public Item(ItemName name, float value, float seconds = -1)
        {
            Name = name;
            Value = value;
            Seconds = seconds;
        }

        public enum ItemName
        {
            BonusBadges,

            PlayerDoubleDamageBoost,
            PlayerDoubleShootingSpeedBoost,
            PlayerDoubleArmorBoost,

            PlayerHealHalf,
            PlayerHealFull,

            PlayerPropertyPoint,
            PlayerPropertyLevel,

            TankDoubleDamageBoost,
            TankDoubleShootingSpeedBoost,
            TankDoubleArmorBoost,

            TankHealHalf,
            TankHealFull,

            TankPropertyPoint,
            TankPropertyLevel
        }
    }

    [Serializable]
    public class ItemCount
    {
        public Item Item;
        public int Count;

        public ItemName Name => Item.Name;

        public ItemCount(Item item, int count)
        {
            Item = item;
            Count = count;
        }
    }

    [Serializable]
    public class ShopItem
    {
        public readonly ItemCount[] Items;
        public readonly int Cost;
        public readonly MoneyTypes MoneyType;

        public enum MoneyTypes
        {
            Game,
            Real
        }
    }
}
