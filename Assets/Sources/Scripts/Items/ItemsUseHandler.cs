using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemsUseHandler
    {

        private static Dictionary<ItemType, Action<Item>> Handlers = new Dictionary<ItemType, Action<Item>>
        {
            {ItemType.Badges, UseBonusBadges},

            {ItemType.ArmorMultyplier, UseBoost},
            {ItemType.DamageMultyplier, UseBoost},
            {ItemType.ShootingDelayDivider, UseBoost},

            {ItemType.Healer, UseHealer},

            {ItemType.MaxHealthProperty, UsePropertyPoint},
            {ItemType.ArmorProperty, UsePropertyPoint},
            {ItemType.DamageProperty, UsePropertyPoint},
            {ItemType.ShootingDelayProperty, UsePropertyPoint},
    };

        public static void UseItem(Item item)
        {
            if(Handlers.TryGetValue(item.Type, out Action<Item> action))
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
