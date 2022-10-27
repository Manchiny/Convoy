using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Assets.Scripts.UnitPropertyLevels;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "UnitPropertiesDatabase", menuName = "Data/UnitPropertiesDatabase")]
    public class UnitPropertiesDatabase : ScriptableObject
    {
        [SerializeField] private List<UnitPropertyLevels> _properties;

        private Dictionary<UnitPropertyType, UnitPropertyLevels> _unitProperties;

        public float GetPropertyValueByLevel(UnitPropertyType type, int level)
        {
            if (_unitProperties == null)
                Init();

            if(GetPropertyValuesByType(type, out UnitPropertyLevels levels))
                return levels.GetByLevel(level).Value;
            else
                return 0;
        }

        public int LevelsCount(UnitPropertyType type)
        {
            if (GetPropertyValuesByType(type, out UnitPropertyLevels levels))
                return levels.Levels.Count;

            else return 0;
        }

        private void Init()
        {
            FillKeys();

            //SetLevelValues(_armorLevels);
            //SetLevelValues(_damageLevels);
            //SetLevelValues(_shootDelayLevels);
            //SetLevelValues(_maxHealthLevels);
        }

        private void FillKeys()
        {
            _unitProperties = new();

            foreach (var property in _properties)
            {
                if(_unitProperties.TryAdd(property.PropertyType, property) == false)
                {
#if UNITY_EDITOR
                    Debug.LogError($"{name} incorrect property type {property.PropertyType}! Probably there is a dubbing in the database.");
#endif
                }
            }
#if UNITY_EDITOR

            if (_unitProperties.Count != _properties.Count)
                Debug.LogError($"{name} database in not full or has dubbing types!");
#endif
        }

        private bool GetPropertyValuesByType(UnitPropertyType type, out UnitPropertyLevels levels)
        {
            if (_unitProperties.TryGetValue(type, out levels))
                return true;
            else
            {
                Debug.LogError($"{name} don't contain levels by type {type}!");
                return false;
            }
        }
    }

    [Serializable]
    public class UnitPropertyLevels
    {
        [SerializeField] private UnitPropertyType _type;
        [SerializeField] private List<UnitPropertyLevel> _levels;

        public UnitPropertyType PropertyType => _type;
        public IReadOnlyList<UnitPropertyLevel> Levels => _levels;

        public enum UnitPropertyType
        {
            Armor,
            Damage,
            ShootDelay,
            MaxHealth
        }

        public UnitPropertyLevel GetByLevel(int level)
        {
            if (level < _levels.Count)
                return _levels[level];
            else
                return _levels.Last();
        }

        //private void SetLevelValues()
        //{
        //    for (int i = 0; i < _levels.Count; i++)
        //    {
        //        _levels[i].SetLevel(i);
        //    }
        //}
    }

    [Serializable]
    public class UnitPropertyLevel
    {
        [SerializeField] private float _value;
        public float Value => _value;
    }
}
