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

            {ItemName.PlayerPropertyPoint, UsePropertyPoint},
            {ItemName.PlayerPropertyLevel, UsePropertyLevel},

            {ItemName.TankDoubleArmorBoost, UseBoost},
            {ItemName.TankDoubleDamageBoost, UseBoost},
            {ItemName.TankDoubleShootingSpeedBoost, UseBoost},

            {ItemName.TankHeal20, UseHealer},

            {ItemName.TankPropertyPoint, UsePropertyPoint},
            {ItemName.TankPropertyLevel, UsePropertyLevel}
    };

        public static void UseItem(Item item)
        {
            if(Handlers.TryGetValue(item.Name, out Action<Item> action))
            {
                action?.Invoke(item);
                Debug.Log($"Item effect from {item.Name} started!");
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