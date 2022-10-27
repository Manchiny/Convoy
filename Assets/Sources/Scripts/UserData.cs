using System;
using System.Collections.Generic;
using System.Linq;
using static Assets.Scripts.UnitPropertyLevels;

namespace Assets.Scripts
{
    [Serializable]
    public class UserData
    {
        public UnitData TankData;
        public UnitData PlayerCharacterData;

        public int LevelId;
        public int Badges;
    }

    [Serializable]
    public class UnitData
    {
        public UnitPropertyValues ArmorProperty;
        public UnitPropertyValues DamageProperty;
        public UnitPropertyValues ShootDelayProperty;
        public UnitPropertyValues MaxHealthProperty;

        public event Action Changed;

        private Dictionary<UnitPropertyType, UnitPropertyValues> _values;

        public UnitData()
        {
            ArmorProperty = new();
            DamageProperty = new();
            ShootDelayProperty = new();
            MaxHealthProperty = new();
        }

        public UnitData(int level)
        {
            ArmorProperty = new(level);
            DamageProperty = new(level);
            ShootDelayProperty = new(level);
            MaxHealthProperty = new(level);
        }

        public float GetPropertyValue(UnitPropertyType type, UnitPropertiesDatabase database)
        {
            if (_values == null)
                FillValues();

            return database.GetPropertyValueByLevel(type, GetUserPropertyLevel(type));
        }

        public void AddUpgradePoint(UnitPropertyType stat)
        {
            switch (stat)
            {
                case UnitPropertyType.Armor:
                    ArmorProperty.AddUpgradePoint();
                    break;
                case UnitPropertyType.Damage:
                    DamageProperty.AddUpgradePoint();
                    break;
                case UnitPropertyType.ShootDelay:
                    ShootDelayProperty.AddUpgradePoint();
                    break;
                case UnitPropertyType.MaxHealth:
                    MaxHealthProperty.AddUpgradePoint();
                    break;
            }

            Changed?.Invoke();
        }

        private void FillValues()
        {
            if (MaxHealthProperty == null)
                MaxHealthProperty = new();
            if (ArmorProperty == null)
                ArmorProperty = new();
            if (DamageProperty == null)
                DamageProperty = new();
            if (ShootDelayProperty == null)
                ShootDelayProperty = new();

            _values = new();

            _values.Add(UnitPropertyType.MaxHealth, MaxHealthProperty);
            _values.Add(UnitPropertyType.Armor, ArmorProperty);
            _values.Add(UnitPropertyType.Damage, DamageProperty);
            _values.Add(UnitPropertyType.ShootDelay, ShootDelayProperty);
        }

        private int GetUserPropertyLevel(UnitPropertyType type)
        {
            if (_values.TryGetValue(type, out UnitPropertyValues property))
                return property.LevelValue;

            return 0;
        }
    }

    [Serializable]
    public class UnitPropertyValues
    {
        private const int MaxUpgradePoints = 3;

        public int LevelValue;
        public int UpgradePoints;

        public event Action LevelChanged;

        public UnitPropertyValues() { }

        public UnitPropertyValues(int level)
        {
            LevelValue = level;
        }

        public void AddUpgradePoint()
        {
            UpgradePoints++;

            if (UpgradePoints >= MaxUpgradePoints)
            {
                UpgradePoints = 0;
                LevelValue++;
                LevelChanged?.Invoke();
            }
        }
    }
}
