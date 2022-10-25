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
    public class TankData
    {
        public int ArmorLevel;
        public int DamageLevel;
        public int ShootDelayLevel;

        private TankPropertiesDatabase _database;

        public TankData(TankPropertiesDatabase database)
        {
            _database = database;

            ArmorLevel = 0;
            DamageLevel = 0;
            ShootDelayLevel = 0;
        }

        public int Armor => (int)_database.ArmorLevels.Where(property => property.Level == ArmorLevel).FirstOrDefault().Value;
        public int Damage => (int)_database.DamageLevels.Where(property => property.Level == DamageLevel).FirstOrDefault().Value;
        public float ShootDelay => _database.ShootDelayLevels.Where(property => property.Level == ShootDelayLevel).FirstOrDefault().Value;
    }
}
