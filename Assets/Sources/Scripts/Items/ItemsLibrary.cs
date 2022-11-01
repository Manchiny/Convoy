using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemsLibrary
    {
        private const float BoostEffectTime = 30f;

        private static readonly List<Item> Items = new List<Item>
        {
            {new Item(ItemName.BonusBadges, ItemOwner.Common, 25f)},

            {new Item(ItemName.PlayerDoubleArmorBoost, ItemOwner.Player, 2f, ItemType.ArmorMultyplier,  BoostEffectTime)},
            {new Item(ItemName.PlayerDoubleDamageBoost,ItemOwner.Player, 2f, ItemType.DamageMultyplier, BoostEffectTime)},
            { new Item(ItemName.PlayerDoubleShootingSpeedBoost, ItemOwner.Player, 2f, ItemType.ShootingDelayDivider, BoostEffectTime)},

            { new Item(ItemName.PlayerHeal20, ItemOwner.Player, 0.2f)},

            { new Item(ItemName.PlayerPropertyPointMaxHealth, ItemOwner.Player, 1, ItemType.MaxHealth)},
            { new Item(ItemName.PlayerPropertyPointArmor, ItemOwner.Player, 1, ItemType.Armor)},
            { new Item(ItemName.PlayerPropertyPointDamage, ItemOwner.Player, 1, ItemType.Damage)},
            { new Item(ItemName.PlayerPropertyPointShootingSpeed, ItemOwner.Player, 1, ItemType.ShootingDelay)},

            { new Item(ItemName.PlayerPropertyLevelMaxHealth, ItemOwner.Player, 1, ItemType.MaxHealth)},
            { new Item(ItemName.PlayerPropertyLevelArmor, ItemOwner.Player, 1, ItemType.Armor)},
            { new Item(ItemName.PlayerPropertyLevelDamage, ItemOwner.Player, 1, ItemType.Damage)},
            { new Item(ItemName.PlayerPropertyLevelShootingSpeed, ItemOwner.Player, 1, ItemType.ShootingDelay)},

            { new Item(ItemName.TankDoubleArmorBoost, ItemOwner.Tank,2f, ItemType.ArmorMultyplier, BoostEffectTime)},
            { new Item(ItemName.TankDoubleDamageBoost, ItemOwner.Tank, 2f, ItemType.DamageMultyplier,BoostEffectTime)},
            { new Item(ItemName.TankDoubleShootingSpeedBoost, ItemOwner.Tank, 2f, ItemType.ShootingDelayDivider, BoostEffectTime)},

            { new Item(ItemName.TankHeal20, ItemOwner.Tank, 0.5f)},

            { new Item(ItemName.TankPropertyPointMaxHealth, ItemOwner.Tank, 1, ItemType.MaxHealth)},
            { new Item(ItemName.TankPropertyPointArmor, ItemOwner.Tank, 1, ItemType.Armor)},
            { new Item(ItemName.TankPropertyPointDamage, ItemOwner.Tank, 1, ItemType.Damage)},
            { new Item(ItemName.TankPropertyPointShootingSpeed, ItemOwner.Tank, 1, ItemType.ShootingDelay)},

            { new Item(ItemName.TankPropertyLevelMaxHealth, ItemOwner.Tank, 1, ItemType.MaxHealth)},
            { new Item(ItemName.TankPropertyLevelArmor, ItemOwner.Tank, 1, ItemType.Armor)},
            { new Item(ItemName.TankPropertyLevelDamage, ItemOwner.Tank, 1, ItemType.Damage)},
            { new Item(ItemName.TankPropertyLevelShootingSpeed, ItemOwner.Tank, 1, ItemType.ShootingDelay)},
        };

        public static Item GetItem(ItemName name)
        {
            Item item = Items.Where(item => item.Name == name).FirstOrDefault();

            if(item == null)
                Debug.Log("Items library don't content item " + name);

            return item;
        }
    }
}
