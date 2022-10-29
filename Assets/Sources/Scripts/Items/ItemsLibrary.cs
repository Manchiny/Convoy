using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Items.Item;

namespace Assets.Scripts.Items
{
    public class ItemsLibrary
    {
        public static readonly Dictionary<ItemName, Item> Items = new Dictionary<ItemName, Item>
        {
            {ItemName.BonusBadges, new Item(ItemName.BonusBadges, 25f)},

            {ItemName.PlayerDoubleArmor, new Item(ItemName.PlayerDoubleArmor, 2f, 30f)},
            {ItemName.PlayerDoubleDamageBoost, new Item(ItemName.PlayerDoubleDamageBoost, 2f, 30f)},
            {ItemName.PlayerDoubleShootingSpeedBoost, new Item(ItemName.PlayerDoubleShootingSpeedBoost, 2f, 30f)},

            {ItemName.PlayerHealHalf, new Item(ItemName.PlayerHealHalf, 0.5f)},
            {ItemName.PlayerHealFull, new Item(ItemName.PlayerHealFull, 1f)},

            {ItemName.PlayerPropertyPoint, new Item(ItemName.PlayerPropertyPoint, 1)},
            {ItemName.PlayerPropertyLevel, new Item(ItemName.PlayerPropertyLevel, 1)},

            {ItemName.TankDoubleArmor, new Item(ItemName.TankDoubleArmor, 2f, 30f)},
            {ItemName.TankDoubleDamageBoost, new Item(ItemName.TankDoubleDamageBoost, 2f, 30f)},
            {ItemName.TankDoubleShootingSpeedBoost, new Item(ItemName.TankDoubleShootingSpeedBoost, 2f, 30f)},

            {ItemName.TankHealHalf, new Item(ItemName.TankHealHalf, 0.5f)},
            {ItemName.TankHealFull, new Item(ItemName.TankHealFull, 1f)},

            {ItemName.TankPropertyPoint, new Item(ItemName.TankPropertyPoint, 1f)},
            {ItemName.TankPropertyLevel, new Item(ItemName.TankPropertyLevel, 1f)}
        };

        public static Item GetItem(ItemName name)
        {
            if (Items.TryGetValue(name, out Item item) == true)
                return item;

            Debug.Log("Items library don't content item " + name);
            return null;
        }
    }
}
