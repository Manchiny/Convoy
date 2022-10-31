using System.Collections.Generic;
using UnityEngine;

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

            {ItemName.PlayerHeal20, new Item(ItemName.PlayerHeal20, ItemOwner.Player, 0.2f)},

            {ItemName.PlayerPropertyPointMaxHealth, new Item(ItemName.PlayerPropertyPointMaxHealth, ItemOwner.Player, 1, ItemType.MaxHealth)},
            {ItemName.PlayerPropertyPointArmor, new Item(ItemName.PlayerPropertyPointArmor, ItemOwner.Player, 1, ItemType.Armor)},
            {ItemName.PlayerPropertyPointDamage, new Item(ItemName.PlayerPropertyPointDamage, ItemOwner.Player, 1, ItemType.Damage)},
            {ItemName.PlayerPropertyPointShootingSpeed, new Item(ItemName.PlayerPropertyPointShootingSpeed, ItemOwner.Player, 1, ItemType.ShootingDelay)},

            {ItemName.PlayerPropertyLevelMaxHealth, new Item(ItemName.PlayerPropertyLevelMaxHealth, ItemOwner.Player, 1, ItemType.MaxHealth)},
            {ItemName.PlayerPropertyLevelArmor, new Item(ItemName.PlayerPropertyLevelArmor, ItemOwner.Player, 1, ItemType.Armor)},
            {ItemName.PlayerPropertyLevelDamage, new Item(ItemName.PlayerPropertyLevelDamage, ItemOwner.Player, 1, ItemType.Damage)},
            {ItemName.PlayerPropertyLevelShootingSpeed, new Item(ItemName.PlayerPropertyLevelShootingSpeed, ItemOwner.Player, 1, ItemType.ShootingDelay)},

            {ItemName.TankDoubleArmorBoost, new Item(ItemName.TankDoubleArmorBoost, ItemOwner.Tank,2f, ItemType.ArmorMultyplier, BoostEffectTime)},
            {ItemName.TankDoubleDamageBoost, new Item(ItemName.TankDoubleDamageBoost, ItemOwner.Tank, 2f, ItemType.DamageMultyplier,BoostEffectTime)},
            {ItemName.TankDoubleShootingSpeedBoost, new Item(ItemName.TankDoubleShootingSpeedBoost, ItemOwner.Tank, 2f, ItemType.ShootingDelayDivider, BoostEffectTime)},

            {ItemName.TankHeal20, new Item(ItemName.TankHeal20, ItemOwner.Tank, 0.5f)},

            {ItemName.TankPropertyPointMaxHealth, new Item(ItemName.PlayerPropertyPointMaxHealth, ItemOwner.Tank, 1, ItemType.MaxHealth)},
            {ItemName.TankPropertyPointArmor, new Item(ItemName.PlayerPropertyPointArmor, ItemOwner.Tank, 1, ItemType.Armor)},
            {ItemName.TankPropertyPointDamage, new Item(ItemName.PlayerPropertyPointDamage, ItemOwner.Tank, 1, ItemType.Damage)},
            {ItemName.TankPropertyPointShootingSpeed, new Item(ItemName.PlayerPropertyPointShootingSpeed, ItemOwner.Tank, 1, ItemType.ShootingDelay)},

            {ItemName.TankPropertyLevelMaxHealth, new Item(ItemName.TankPropertyLevelMaxHealth, ItemOwner.Tank, 1, ItemType.MaxHealth)},
            {ItemName.TankPropertyLevelArmor, new Item(ItemName.TankPropertyLevelArmor, ItemOwner.Tank, 1, ItemType.Armor)},
            {ItemName.TankPropertyLevelDamage, new Item(ItemName.TankPropertyLevelDamage, ItemOwner.Tank, 1, ItemType.Damage)},
            {ItemName.TankPropertyLevelShootingSpeed, new Item(ItemName.TankPropertyLevelShootingSpeed, ItemOwner.Tank, 1, ItemType.ShootingDelay)},
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
