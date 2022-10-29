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

            {ItemName.PlayerHealHalf, UseHealer},
            {ItemName.PlayerHealFull, UseHealer},

            {ItemName.PlayerPropertyPoint, UsePropertyPoint},
            {ItemName.PlayerPropertyLevel, UsePropertyLevel},

            {ItemName.TankDoubleArmorBoost, UseBoost},
            {ItemName.TankDoubleDamageBoost, UseBoost},
            {ItemName.TankDoubleShootingSpeedBoost, UseBoost},

            {ItemName.TankHealHalf, UseHealer},
            {ItemName.TankHealFull, UseHealer},

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
            IBoostable unit = item.Owner == ItemOwner.Tank ? Game.Tank : Game.Player;
            unit.AddBoost(item.Type, item.Value);
        }

        private static void UseHealer(Item item)
        {

        }

        private static void UsePropertyPoint(Item item)
        {

        }

        private static void UsePropertyLevel(Item item)
        {

        }

    }
}
