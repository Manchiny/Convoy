using Assets.Scripts.Destroyable;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Tower : Damageable, IAttackable
    {
        [SerializeField] private ShelterEnemy _unit;
        [SerializeField] private DestroyableObject _destroyable;

        public override Team TeamId => _unit.TeamId;

        public override int MaxHealth => 100;
        public override int Armor => 0;
        public int Damage => 0;
        public float ShootDelay => 0f;

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
            _unit.transform.parent = transform.parent;
            _unit.ForceDie();

            _destroyable.transform.parent = transform.parent;
            _destroyable.DestroyObject();

            gameObject.SetActive(false);
        }

        protected override void OnGetDamage()
        {
            
        }
    }
}
