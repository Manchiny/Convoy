using Assets.Scripts.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Items.Item;

namespace Assets.Scripts.Items
{
    public class ItemsUseHandler
    {
        private Unit _player;
        private Unit _tank;

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

        public ItemsUseHandler()
        {
            _player = Game.Player;
            _tank = Game.Tank;
        }

        public static void UseItem(Item item)
        {

        }

        private static void UseBonusBadges(Item item)
        {

        }

        private static void UseBoost(Item item)
        {

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
