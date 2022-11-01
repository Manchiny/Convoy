using Assets.Scripts.Guns;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Assets.Scripts.UnitPropertyLevels;

namespace Assets.Scripts.Units
{
    public abstract class Unit : Damageable, IAttackable
    {
        [SerializeField] protected Gun Gun;

        private UnitPropertiesDatabase _propertiesDatabase;
        private HashSet<Damageable> _attackTargets = new();

        private int _baseArmor;
        private int _baseDamage;
        private float _baseShootDelay;

        public virtual Damageable Target { get; protected set; }
        public UnitData Data { get; private set; }

        public sealed override int MaxHealth { get; protected set; }
        public override int Armor => _baseArmor;
        public virtual int Damage => _baseDamage;
        public virtual float ShootDelay => _baseShootDelay;

        public UnitPropertiesDatabase PropertiesDatabase => _propertiesDatabase;

        public void InitData(UnitData data, UnitPropertiesDatabase propertiesDatabase)
        {
            Data = data;
            _propertiesDatabase = propertiesDatabase;

            UpdateDataProperties();
        }

        public void UpdateDataProperties()
        {
            _baseArmor = (int)Data.GetPropertyValue(UnitPropertyType.Armor, _propertiesDatabase);
            _baseDamage = (int)Data.GetPropertyValue(UnitPropertyType.Damage, _propertiesDatabase);
            _baseShootDelay = Data.GetPropertyValue(UnitPropertyType.ShootDelay, _propertiesDatabase);
            MaxHealth = (int)Data.GetPropertyValue(UnitPropertyType.MaxHealth, _propertiesDatabase);
        }

        public void AddFindedEnemy(Damageable enemy)
        {
            _attackTargets.Add(enemy);
            OnEnemyFinded(enemy);
        }

        public void RemoveFromEnemies(Damageable enemy)
        {
            _attackTargets.Remove(enemy);
            OnEnenmyMissed(enemy);
        }

        public override void OnRestart()
        {
            base.OnRestart();
            ClearTargets();
        }

        public void AddPropertyUpgradePoint(UnitPropertyType propertyType)
        {
            Data.AddUpgradePoint(propertyType, PropertiesDatabase);
        }

        public void AddPropertyLevel(UnitPropertyType propertyType)
        {
            Data.UpgradeLevel(propertyType, PropertiesDatabase);
        }

        protected virtual void OnDataInited() { }
        protected abstract void OnEnemyFinded(Damageable enemy);
        protected abstract void OnEnenmyMissed(Damageable enemy);

        protected void TryShoot()
        {
            if (Target.gameObject.activeInHierarchy == false || Target.IsAlive == false)
                RemoveFromEnemies(Target);

            if (Target == null)
                return;

            Gun.TryShoot(Target, TeamId);
        }

        protected Damageable TryGetAnyNewTarget()
        {
            if (_attackTargets.Count > 0)
                return _attackTargets.FirstOrDefault();
            else
                return null;
        }

        protected Damageable TryGetNearestTarget()
        {
            if (_attackTargets.Count == 0)
                return null;

            return _attackTargets.Where(enemy => enemy.gameObject.activeInHierarchy).OrderBy(enemy => (enemy.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
        }

        protected void ClearTargets()
        {
            _attackTargets.Clear();
            Target = null;
        }
    }
}
