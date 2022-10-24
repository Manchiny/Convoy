using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class UserData
    {
        public TankData TankData;
        public int Tokens;

        public void SaveData(UserData data)
        {
            TankData = data.TankData;
            Tokens = data.Tokens;
        }
    }


    [Serializable]
    public class TankData
    {
        public int ArmorLevel;
        public int DamageLevel;
        public int ShootDelayLevel;

        private TankProperiesDatabase _database;

        public TankData(TankProperiesDatabase database)
        {
            _database = database;
        }

        public int Armor => (int)_database.ArmorLevels.Where(property => property.Level == ArmorLevel).FirstOrDefault().Value;
        public int Damage => (int)_database.DamageLevels.Where(property => property.Level == DamageLevel).FirstOrDefault().Value;
        public float ShootDelay => (int)_database.ShootDelayLevels.Where(property => property.Level == ShootDelayLevel).FirstOrDefault().Value;
    }


    [CreateAssetMenu(fileName = "TankProperiesDatabase", menuName = "Data/TankProperiesDatabase")]
    public class TankProperiesDatabase : ScriptableObject
    {
        [SerializeField] private List<UnitProperty> _armorLevels;
        [SerializeField] private List<UnitProperty> _damageLevels;
        [SerializeField] private List<UnitProperty> _shootDelayLevels;

        public IReadOnlyList<UnitProperty> ArmorLevels => _armorLevels;
        public IReadOnlyList<UnitProperty> DamageLevels => _damageLevels;
        public IReadOnlyList<UnitProperty> ShootDelayLevels => _shootDelayLevels;

        public void Init()
        {
            Validate();

            SetLevelValues(_armorLevels);
            SetLevelValues(_damageLevels);
            SetLevelValues(_shootDelayLevels);
        }

        private void Validate()
        {
            _armorLevels = _armorLevels.OrderBy(property => property.Value).ToList();
            _damageLevels = _armorLevels.OrderBy(property => property.Value).ToList();
            _shootDelayLevels = _armorLevels.OrderByDescending(property => property.Value).ToList();
        }

        private void SetLevelValues(List<UnitProperty> properties)
        {
            for (int i = 0; i < _armorLevels.Count; i++)
            {
                _armorLevels[i].SetLevel(i);
            }
        }
    }

    [Serializable]
    public class UnitProperty
    {
        [SerializeField] private float _value;

        private int _level;
        public int Level => _level;
        public float Value => _value;

        public void SetLevel(int level)
        {
            _level = level;
        }
    }
}
