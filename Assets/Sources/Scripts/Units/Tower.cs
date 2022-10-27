using Assets.Scripts.Destroyable;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Tower : Damageable, IAttackable, IRestartable, IOutOfRoad
    {
        [SerializeField] private TowerSolderEnemy _unit;
        [SerializeField] private DestroyableObject _destroyable;

        public override Team TeamId => _unit.TeamId;
        public Unit Unit => _unit;

        public override int MaxHealth { get; protected set; } = 100;
        public override int Armor { get; protected set; } = 0;
        public int Damage => 0;
        public float ShootDelay => 0f;

        public void AddFindedEnemy(Damageable enemy) // TODO: избавиться здесь от интерфейса IAttackable
        {
            _unit.AddFindedEnemy(enemy);
        }

        public void RemoveFromEnemies(Damageable enemy)
        {
            _unit.RemoveFromEnemies(enemy);
        }

        public override void OnRestart()
        {
            base.OnRestart();
            _destroyable.transform.parent = transform;
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
