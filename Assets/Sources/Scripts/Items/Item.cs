using System;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class Item
    {
        public readonly ItemName Name;
        public readonly float Value;
        public readonly float Seconds;

        public bool IsTemporary;

        public Item(ItemName name, float value, float seconds = -1)
        {

        }

        public enum ItemName
        {
            BonusBadges,

            PlayerDoubleDamageBoost,
            PlayerDoubleShootingSpeedBoost,
            PlayerDoubleArmor,

            PlayerHealHalf,
            PlayerHealFull,

            PlayerPropertyPoint,
            PlayerPropertyLevel,

            TankDoubleDamageBoost,
            TankDoubleShootingSpeedBoost,
            TankDoubleArmor,

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
    }

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
