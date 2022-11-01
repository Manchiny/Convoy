using Assets.Scripts.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.UnitPropertyLevels;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class Item
    {
        public ItemName Name;

        public readonly ItemOwner Owner;
        public readonly ItemType Type;
        public readonly float Value;
        public readonly float Seconds;

        private static Dictionary<ItemType, UnitPropertyType> UnitPropertiesByItemType = new()
        {
            { ItemType.ArmorMultyplier, UnitPropertyType.Armor },
            { ItemType.DamageMultyplier, UnitPropertyType.Damage },
            { ItemType.ShootingDelayDivider, UnitPropertyType.ShootDelay },

            { ItemType.MaxHealth, UnitPropertyType.MaxHealth },
            { ItemType.Armor, UnitPropertyType.Armor },
            { ItemType.Damage, UnitPropertyType.Damage },
            { ItemType.ShootingDelay, UnitPropertyType.ShootDelay }
        };

        private float _coolDown;
        private Coroutine _effectAction;

        public bool IsTemporary => Seconds > 0;
        public bool CanUse => IsTemporary == false || _coolDown <= 0;

        public Item(ItemName name, ItemOwner owner, float value, ItemType type = ItemType.Defualt, float seconds = -1)
        {
            Name = name;
            Value = value;
            Seconds = seconds;
            Owner = owner;
            Type = type;
        }

        ~Item()
        {
            if (_effectAction != null)
                Game.Instance.StopCoroutine(_effectAction);
        }

        public static UnitPropertyType UnitPropertyByItemType(ItemType itemType)
        {
            if (UnitPropertiesByItemType.TryGetValue(itemType, out UnitPropertyType type) == false)
                Debug.LogError($"UnitPropertiesByItemType don't content value for {itemType}");

            return type;
        }

        public void OnUse(Action onEffectEnd)
        {
            if (IsTemporary)
            {
                _coolDown = Seconds;
                _effectAction = Game.Instance.StartCoroutine(StartEffectAction(onEffectEnd));
            }
            else
            {
                if (onEffectEnd != null)
                    onEffectEnd?.Invoke();
            }
        }

        private IEnumerator StartEffectAction(Action onEnd)
        {
            yield return new WaitForSeconds(Seconds);
            _coolDown = 0;

            IBoostable unit = null;

            if (Owner == ItemOwner.Player)
                unit = Game.Player;
            else if (Owner == ItemOwner.Tank)
                unit = Game.Tank;

            if (unit != null)
                unit.RemoveBoost(Type);

            if (onEnd != null)
                onEnd?.Invoke();
        }
    }

    [Serializable]
    public class ItemCount
    {
        public Item Item;
        public int Count;
        public ItemName Name => Item.Name;

        public ItemCount(Item item, int count)
        {
            Item = item;
            Count = count;
        }
    }

    public enum ItemName
    {
        BonusBadges,

        PlayerDoubleDamageBoost,
        PlayerDoubleShootingSpeedBoost,
        PlayerDoubleArmorBoost,

        PlayerHeal20,

        PlayerPropertyPointMaxHealth,
        PlayerPropertyPointArmor,
        PlayerPropertyPointDamage,
        PlayerPropertyPointShootingSpeed,

        PlayerPropertyLevelMaxHealth,
        PlayerPropertyLevelArmor,
        PlayerPropertyLevelDamage,
        PlayerPropertyLevelShootingSpeed,

        TankDoubleDamageBoost,
        TankDoubleShootingSpeedBoost,
        TankDoubleArmorBoost,

        TankHeal20,

        TankPropertyPointMaxHealth,
        TankPropertyPointArmor,
        TankPropertyPointDamage,
        TankPropertyPointShootingSpeed,

        TankPropertyLevelMaxHealth,
        TankPropertyLevelArmor,
        TankPropertyLevelDamage,
        TankPropertyLevelShootingSpeed,
    }

    public enum ItemOwner
    {
        Common,
        Player,
        Tank
    }

    public enum ItemType
    {
        Defualt,
        DamageMultyplier,
        ArmorMultyplier,
        ShootingDelayDivider,

        Armor,
        MaxHealth,
        ShootingDelay,
        Damage,
    }
}
