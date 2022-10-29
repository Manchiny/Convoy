using Assets.Scripts.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class UnitBoosts : MonoBehaviour
    {
        private IBoostable _boostable;
        private Dictionary<ItemType, float> _currentBoosts = new();

        private void Awake()
        {
            _boostable = GetComponent<IBoostable>();
            _boostable.InitUnitBoosts(this);
        }

        public bool TryGetBoostValue(ItemType type, out float value)
        {
            if (_currentBoosts.TryGetValue(type, out value))
                return true;

            return false;
        }

        public bool TryAddBoost(ItemType type, float value)
        {
            if (_currentBoosts.TryAdd(type, value) == false)
            {
                Debug.LogError($"Error on trying add boost type {type}; Unit already contain it!");
                return false;
            }
            else
                return true;
        }

        public void RemoveBoost(ItemType type)
        {
            if (_currentBoosts.ContainsKey(type))
                _currentBoosts.Remove(type);
        }
    }
}

