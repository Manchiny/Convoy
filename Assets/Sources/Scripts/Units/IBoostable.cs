using Assets.Scripts.Items;
using System;

namespace Assets.Scripts.Units
{
    public interface IBoostable 
    {
        public event Action<ItemType> BoostAdded;
        public event Action<ItemType> BoostRemoved;

        public void InitUnitBoosts(UnitBoosts boosts);
        public void AddBoost(ItemType type, float value);
        public void RemoveBoost(ItemType type);
        public void PlayBoosUseEffect();
        public void PlayHealerUseEffect();
    }
}