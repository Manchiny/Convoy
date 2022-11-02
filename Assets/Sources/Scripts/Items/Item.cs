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
        [SerializeField] private bool _isAutoUse = false;
        [SerializeField] private ItemName _name;
        [SerializeField] private ItemOwner _owner;
        [SerializeField] private ItemType _type;
        [Space]
        [SerializeField] private float _value;
        [SerializeField] private float _boostSeconds;
        [Space]
        [SerializeField] private Sprite _icon;

        private static Dictionary<ItemType, UnitPropertyType> UnitPropertiesByItemType = new()
        {
            { ItemType.ArmorMultyplier, UnitPropertyType.Armor },
            { ItemType.DamageMultyplier, UnitPropertyType.Damage },
            { ItemType.ShootingDelayDivider, UnitPropertyType.ShootDelay },

            { ItemType.MaxHealthProperty, UnitPropertyType.MaxHealth },
            { ItemType.ArmorProperty, UnitPropertyType.Armor },
            { ItemType.DamageProperty, UnitPropertyType.Damage },
            { ItemType.ShootingDelayProperty, UnitPropertyType.ShootDelay }
        };

        private float _coolDown;
        private Coroutine _effectAction;

        ~Item()
        {
            if (_effectAction != null)
                Game.Instance.StopCoroutine(_effectAction);
        }

        public ItemType Type => _type;
        public ItemName Name => _name;
        public ItemOwner Owner => _owner;

        public float Value => _value;
        public float BoostSeconds => _boostSeconds;
        public Sprite Icon => _icon;
        public bool IsTemporary => BoostSeconds > 0;
        public bool CanUse => IsTemporary == false || _coolDown <= 0;
        public bool IsAutoUse => _isAutoUse;

        public static UnitPropertyType UnitPropertyByItemType(ItemType itemType)
        {
            if (UnitPropertiesByItemType.TryGetValue(itemType, out UnitPropertyType type) == false)
                Debug.LogError($"UnitPropertiesByItemType don't content value for {itemType}");

            return type;
        }

        public string GetValueFormatedString()
        {
            if (IsHealer)
                return $"+{Value}%";
            else if (IsBoost)
                return $"x{Value}";
            else if (IsPropertyPoint)
                return $"1 {"property_point".Localize()}";
            else
                return "";
        }

        public void OnUse(Action onEffectEnd)
        {
            if (IsTemporary)
            {
                _coolDown = BoostSeconds;
                _effectAction = Game.Instance.StartCoroutine(StartEffectAction(onEffectEnd));
            }
            else
            {
                if (onEffectEnd != null)
                    onEffectEnd?.Invoke();
            }
        }

        public bool IsHealer => Type == ItemType.Healer;
        public bool IsBoost => Type == ItemType.ArmorMultyplier || Type == ItemType.DamageMultyplier || Type == ItemType.ShootingDelayDivider;
        public bool IsPropertyPoint => Type == ItemType.MaxHealthProperty || Type == ItemType.DamageProperty || Type == ItemType.ArmorProperty || Type == ItemType.ShootingDelayProperty;

        private IEnumerator StartEffectAction(Action onEnd)
        {
            yield return new WaitForSeconds(BoostSeconds);
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
        [SerializeField] private ItemName _name;
        public int Count;

        public Item Item => Game.Shop.ItemsDatabase.GetItem(_name);

        public ItemName Name => _name;

        public ItemCount(ItemName name, int count)
        {
            _name = name;
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

        TankDoubleDamageBoost,
        TankDoubleShootingSpeedBoost,
        TankDoubleArmorBoost,

        TankHeal20,

        TankPropertyPointMaxHealth,
        TankPropertyPointArmor,
        TankPropertyPointDamage,
        TankPropertyPointShootingSpeed,
    }

    public enum ItemOwner
    {
        Common,
        Player,
        Tank
    }

    public enum ItemType
    {
        Unknown,
        Badges,
        Healer,

        DamageMultyplier,
        ArmorMultyplier,
        ShootingDelayDivider,

        ArmorProperty,
        MaxHealthProperty,
        ShootingDelayProperty,
        DamageProperty,
    }
}
