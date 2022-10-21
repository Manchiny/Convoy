using UnityEngine;

namespace Assets.Scripts.Units
{

    public class Tower : Damageable, IAttackable
    {
        [SerializeField] private ShelterEnemy _unit;

        public override int MaxHealth => 100;
        public override Team TeamId => _unit.TeamId;

        public void AddFindedEnemy(Damageable enemy)
        {
            _unit.AddFindedEnemy(enemy);
        }

        public void RemoveFromEnemies(Damageable enemy)
        {
            _unit.RemoveFromEnemies(enemy);
        }

        protected override void Die()
        {
            _unit.ForceDie();         
        }

        protected override void OnGetDamage()
        {
            
        }
    }
}
