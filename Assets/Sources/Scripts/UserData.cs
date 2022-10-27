using System;
using System.Linq;

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

        public enum StatName
        {
            Armor,
            Damage,
            ShootDelay,
            MaxHealth
        }

        public UnitData()
        {
            ArmorProperty = new();
            DamageProperty = new();
            ShootDelayProperty = new();
            MaxHealthProperty = new();
        }

        public int GetArmor(UnitPropertiesDatabase database) 
        {
            if (ArmorProperty == null)
                ArmorProperty = new();

           return (int)database.ArmorLevels.Where(property => property.Level == ArmorProperty.LevelValue).FirstOrDefault().Value;
        }


        public int GetDamage(UnitPropertiesDatabase database) 
        {
            if (DamageProperty == null)
                DamageProperty = new();

            return (int)database.DamageLevels.Where(property => property.Level == DamageProperty.LevelValue).FirstOrDefault().Value;
        }


        public float GetShootDelay(UnitPropertiesDatabase database) 
        {
            if (ShootDelayProperty == null)
                ShootDelayProperty = new();

            return database.ShootDelayLevels.Where(property => property.Level == ShootDelayProperty.LevelValue).FirstOrDefault().Value;
        }

        public int GetMaxHealth(UnitPropertiesDatabase database) 
        {
            if (MaxHealthProperty == null)
                MaxHealthProperty = new();

           return (int)database.MaxHealthLevels.Where(property => property.Level == MaxHealthProperty.LevelValue).FirstOrDefault().Value;
        }

        public void AddUpgradePoint(StatName stat)
        {
            switch (stat)
            {
                case StatName.Armor:
                    ArmorProperty.AddUpgradePoint();
                    break;
                case StatName.Damage:
                    DamageProperty.AddUpgradePoint();
                    break;
                case StatName.ShootDelay:
                    ShootDelayProperty.AddUpgradePoint();
                    break;
                case StatName.MaxHealth:
                    MaxHealthProperty.AddUpgradePoint();
                    break;
            }
        }
    }

    [Serializable]
    public class UnitPropertyValues
    {
        private const int MaxUpgradePoints = 3;

        public int LevelValue;
        public int UpgradePoints;

        public void AddUpgradePoint()
        {
            UpgradePoints++;

            if (UpgradePoints >= MaxUpgradePoints)
            {
                UpgradePoints = 0;
                LevelValue++;
            }
        }
    }
}
