using Assets.Scripts.Items;

namespace Assets.Scripts.Units
{
    public interface IBoostable 
    {
        public void InitUnitBoosts(UnitBoosts boosts);
        public void AddBoost(ItemType type, float value);
        public void RemoveBoost(ItemType type);
        public void PlayBoosUseEffect();
        public void PlayHealerUseEffect();
    }
}