using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Items.Item;

namespace Assets.Scripts.Items
{
    public class ItemsLibrary
    {
        private const float BoostEffectTime = 30f;

        public static readonly Dictionary<ItemName, Item> Items = new Dictionary<ItemName, Item>
        {
            {ItemName.BonusBadges, new Item(ItemName.BonusBadges, ItemOwner.Common, 25f)},

            {ItemName.PlayerDoubleArmorBoost, new Item(ItemName.PlayerDoubleArmorBoost, ItemOwner.Player, 2f, ItemType.ArmorMultyplier,  BoostEffectTime)},
            {ItemName.PlayerDoubleDamageBoost, new Item(ItemName.PlayerDoubleDamageBoost,ItemOwner.Player, 2f, ItemType.DamageMultyplier, BoostEffectTime)},
            {ItemName.PlayerDoubleShootingSpeedBoost, new Item(ItemName.PlayerDoubleShootingSpeedBoost, ItemOwner.Player, 2f, ItemType.ShootingDelayDivider, BoostEffectTime)},

            {ItemName.PlayerHealHalf, new Item(ItemName.PlayerHealHalf, ItemOwner.Player, 0.5f)},
            {ItemName.PlayerHealFull, new Item(ItemName.PlayerHealFull, ItemOwner.Player, 1f)},

            {ItemName.PlayerPropertyPoint, new Item(ItemName.PlayerPropertyPoint, ItemOwner.Player, 1)},
            {ItemName.PlayerPropertyLevel, new Item(ItemName.PlayerPropertyLevel, ItemOwner.Player, 1)},

            {ItemName.TankDoubleArmorBoost, new Item(ItemName.TankDoubleArmorBoost, ItemOwner.Tank,2f, ItemType.ArmorMultyplier, BoostEffectTime)},
            {ItemName.TankDoubleDamageBoost, new Item(ItemName.TankDoubleDamageBoost, ItemOwner.Tank, 2f, ItemType.DamageMultyplier,BoostEffectTime)},
            {ItemName.TankDoubleShootingSpeedBoost, new Item(ItemName.TankDoubleShootingSpeedBoost, ItemOwner.Tank, 2f, ItemType.ShootingDelayDivider, BoostEffectTime)},

            {ItemName.TankHealHalf, new Item(ItemName.TankHealHalf, ItemOwner.Tank, 0.5f)},
            {ItemName.TankHealFull, new Item(ItemName.TankHealFull,ItemOwner.Tank, 1f)},

            {ItemName.TankPropertyPoint, new Item(ItemName.TankPropertyPoint, ItemOwner.Tank, 1f)},
            {ItemName.TankPropertyLevel, new Item(ItemName.TankPropertyLevel, ItemOwner.Tank, 1f)}
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
