using System;
using System.Collections;
using UnityEngine;
using static Assets.Scripts.Items.Item;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class Item
    {
        public ItemName Name;

        public readonly float Value;
        public readonly float Seconds;

        private float _coolDown;
        private Coroutine _effectAction;

        public bool IsTemporary => Seconds > 0;
        public bool CanUse => IsTemporary == false || _coolDown <= 0;


        public Item(ItemName name, float value, float seconds = -1)
        {
            Name = name;
            Value = value;
            Seconds = seconds;
        }

        ~Item()
        {
            if (_effectAction != null)
                Game.Instance.StopCoroutine(_effectAction);
        }

        public enum ItemName
        {
            BonusBadges,

            PlayerDoubleDamageBoost,
            PlayerDoubleShootingSpeedBoost,
            PlayerDoubleArmorBoost,

            PlayerHealHalf,
            PlayerHealFull,

            PlayerPropertyPoint,
            PlayerPropertyLevel,

            TankDoubleDamageBoost,
            TankDoubleShootingSpeedBoost,
            TankDoubleArmorBoost,

            TankHealHalf,
            TankHealFull,

            TankPropertyPoint,
            TankPropertyLevel
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
            yield return Seconds;
            _coolDown = 0;

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
        public bool CanUse => Count > 0 && Item.CanUse;

        public ItemCount(Item item, int count)
        {
            Item = item;
            Count = count;
        }

        public bool TryUse(Action onEffectEnded)
        {
            if (CanUse)
            {
                Count--;
                Item.OnUse(onEffectEnded);

                return true;
            }

            return false;
        }
    }

    [Serializable]
    public class ShopItem
    {
        public readonly ItemCount[] Items;
        public readonly int Cost;
        public readonly MoneyTypes MoneyType;

        public enum MoneyTypes
        {
            Game,
            Real
        }
    }
}
