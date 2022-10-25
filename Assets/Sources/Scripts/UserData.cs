using System;
using System.Linq;

namespace Assets.Scripts
{
    public class UserData
    {
        public TankData TankData;
        public int Badges;

        public UserData()
        {
            TankData = new TankData(Game.TankProperies);
            Badges = 0;
        }
    }


    [Serializable]
    public class TankData // todo переименовать в unitData и юзеру пихнуть аналогичную штуку
    {
        public UnitPropertyValues ArmorProperty;
        public UnitPropertyValues DamageProperty;
        public UnitPropertyValues ShootDelayProperty;

        private TankPropertiesDatabase _database;

        public enum StatName
        {
            Armor,
            Damage,
            ShootDelay
        }

        public TankData(TankPropertiesDatabase database)
        {
            _database = database;

            ArmorProperty = new();
            DamageProperty = new();
            ShootDelayProperty = new();
        }

        public int Armor => (int)_database.ArmorLevels.Where(property => property.Level == ArmorProperty.LevelValue).FirstOrDefault().Value;
        public int Damage => (int)_database.DamageLevels.Where(property => property.Level == DamageProperty.LevelValue).FirstOrDefault().Value;
        public float ShootDelay => _database.ShootDelayLevels.Where(property => property.Level == ShootDelayProperty.LevelValue).FirstOrDefault().Value;

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
