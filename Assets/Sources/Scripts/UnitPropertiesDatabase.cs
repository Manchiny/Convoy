using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "UnitPropertiesDatabase", menuName = "Data/UnitPropertiesDatabase")]
    public class UnitPropertiesDatabase : ScriptableObject
    {
        [SerializeField] private List<UnitProperty> _armorLevels;
        [SerializeField] private List<UnitProperty> _damageLevels;
        [SerializeField] private List<UnitProperty> _shootDelayLevels;
        [SerializeField] private List<UnitProperty> _maxHealthLevels;

        private List<UnitProperty> ValidatedArmorLevels;
        private List<UnitProperty> ValidatedDamageLevels;
        private List<UnitProperty> ValidatedShootDelayLevels;
        private List<UnitProperty> ValidatedMaxHealthLevels;

        public IReadOnlyList<UnitProperty> ArmorLevels => ValidatedArmorLevels;
        public IReadOnlyList<UnitProperty> DamageLevels => ValidatedDamageLevels;
        public IReadOnlyList<UnitProperty> ShootDelayLevels => ValidatedShootDelayLevels;
        public IReadOnlyList<UnitProperty> MaxHealthLevels => ValidatedMaxHealthLevels;

        public void Init()
        {
            Validate();

            SetLevelValues(_armorLevels);
            SetLevelValues(_damageLevels);
            SetLevelValues(_shootDelayLevels);
        }

        private void Validate()
        {
            ValidatedArmorLevels = _armorLevels.OrderBy(property => property.Value).ToList();
            ValidatedDamageLevels = _damageLevels.OrderBy(property => property.Value).ToList();
            ValidatedShootDelayLevels = _shootDelayLevels.OrderByDescending(property => property.Value).ToList();
            ValidatedMaxHealthLevels = _maxHealthLevels.OrderBy(property => property.Value).ToList();
        }

        private void SetLevelValues(List<UnitProperty> properties)
        {
            for (int i = 0; i < properties.Count; i++)
            {
                properties[i].SetLevel(i);
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
