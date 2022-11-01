using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemsUseHandler
    {

        private static Dictionary<ItemName, Action<Item>> Handlers = new Dictionary<ItemName, Action<Item>>
        {
            {ItemName.BonusBadges, UseBonusBadges},

            {ItemName.PlayerDoubleArmorBoost, UseBoost},
            {ItemName.PlayerDoubleDamageBoost, UseBoost},
            {ItemName.PlayerDoubleShootingSpeedBoost,UseBoost},

            {ItemName.PlayerHeal20, UseHealer},

            {ItemName.PlayerPropertyPointMaxHealth, UsePropertyPoint},
            {ItemName.PlayerPropertyPointArmor, UsePropertyPoint},
            {ItemName.PlayerPropertyPointDamage, UsePropertyPoint},
            {ItemName.PlayerPropertyPointShootingSpeed, UsePropertyPoint},

            {ItemName.PlayerPropertyLevelMaxHealth, UsePropertyLevel},
            {ItemName.PlayerPropertyLevelArmor, UsePropertyLevel},
            {ItemName.PlayerPropertyLevelDamage, UsePropertyLevel},
            {ItemName.PlayerPropertyLevelShootingSpeed, UsePropertyLevel},

            {ItemName.TankDoubleArmorBoost, UseBoost},
            {ItemName.TankDoubleDamageBoost, UseBoost},
            {ItemName.TankDoubleShootingSpeedBoost, UseBoost},

            {ItemName.TankHeal20, UseHealer},

            {ItemName.TankPropertyPointMaxHealth, UsePropertyPoint},
            {ItemName.TankPropertyPointArmor, UsePropertyPoint},
            {ItemName.TankPropertyPointDamage, UsePropertyPoint},
            {ItemName.TankPropertyPointShootingSpeed, UsePropertyPoint},

            {ItemName.TankPropertyLevelMaxHealth, UsePropertyLevel},
            {ItemName.TankPropertyLevelArmor, UsePropertyLevel},
            {ItemName.TankPropertyLevelDamage, UsePropertyLevel},
            {ItemName.TankPropertyLevelShootingSpeed, UsePropertyLevel}
    };

        public static void UseItem(Item item)
        {
            if(Handlers.TryGetValue(item.Name, out Action<Item> action))
            {
                action?.Invoke(item);
                Debug.Log($"Item used {item.Name}!");
            }
            else
            {
                Debug.LogError($"Item handler not exist {item.Name}!");
            }
        }

        private static void UseBonusBadges(Item item)
        {
            Game.Instance.AddBadges((int)item.Value);
        }

        private static void UseBoost(Item item)
        {
            IBoostable unit = GetBoostable(item.Owner);
            unit.AddBoost(item.Type, item.Value);
        }

        private static void UseHealer(Item item)
        {
            Unit unit = GetBoostableUnit(item.Owner);
            float count = item.Value * unit.MaxHealth;
            unit.AddHealth((int)count);
        }

        private static void UsePropertyPoint(Item item)
        {
            Unit unit = GetBoostableUnit(item.Owner);
            unit.AddPropertyUpgradePoint(Item.UnitPropertyByItemType(item.Type));
        }

        private static void UsePropertyLevel(Item item)
        {
            Unit unit = GetBoostableUnit(item.Owner);
            unit.AddPropertyLevel(Item.UnitPropertyByItemType(item.Type));
        }

        private static IBoostable GetBoostable(ItemOwner owner)
        {
            if (owner == ItemOwner.Tank)
                return Game.Tank;

            if (owner == ItemOwner.Player)
                return Game.Player;

            return null;
        }

        private static Unit GetBoostableUnit(ItemOwner owner)
        {
            if (owner == ItemOwner.Tank)
                return Game.Tank;

            if (owner == ItemOwner.Player)
                return Game.Player;

            return null;
        }

    }
}
