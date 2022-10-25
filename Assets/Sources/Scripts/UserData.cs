using System;
using System.Linq;

namespace Assets.Scripts
{
    [Serializable]
    public class UserData
    {
        public UnitData TankData;
        public UnitData PlayerCharacterData;

        public int Badges;
    }

    [Serializable]
    public class UnitData
    {
        public UnitPropertyValues ArmorProperty;
        public UnitPropertyValues DamageProperty;
        public UnitPropertyValues ShootDelayProperty;

        public enum StatName
        {
            Armor,
            Damage,
            ShootDelay
        }

        public UnitData()
        {
            ArmorProperty = new();
            DamageProperty = new();
            ShootDelayProperty = new();
        }

        public int GetArmor(UnitPropertiesDatabase database) => (int)database.ArmorLevels.Where(property => property.Level == ArmorProperty.LevelValue).FirstOrDefault().Value;
        public int GetDamage(UnitPropertiesDatabase database) => (int)database.DamageLevels.Where(property => property.Level == DamageProperty.LevelValue).FirstOrDefault().Value;
        public float GetShootDelay(UnitPropertiesDatabase database) => database.ShootDelayLevels.Where(property => property.Level == ShootDelayProperty.LevelValue).FirstOrDefault().Value;

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
