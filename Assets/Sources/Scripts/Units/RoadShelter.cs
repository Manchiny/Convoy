using Assets.Scripts.Destroyable;
using Assets.Scripts.Levels;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class RoadShelter : Damageable, IRestartable
    {
        [SerializeField] private StayOnPlayceEnemy _solderPrefab;
        [SerializeField] private List<Transform> _spwanPoints;
        [SerializeField] private DestroyableObject _destroyable;

        private HashSet<Enemy> _units = new();

        public override Team TeamId => Team.Enemy;
        public override int MaxHealth { get; protected set; } = 300;
        public override int Armor { get; protected set; } = 0;

        public void CreateSolders(int count, LevelConfig config, UnitPropertiesDatabase solderLevelsDatabase)
        {
            if (count > _spwanPoints.Count)
            {
                Debug.LogError("[RoadShelter] you are trying to create more soldiers than the shelter can hold;");
                count = _spwanPoints.Count;
            }

            EnemyGroup group = new EnemyGroup();

            for (int i = 0; i < count; i++)
            {
                Vector3 position = _spwanPoints[i].position;
                Quaternion rotation = _spwanPoints[i].rotation;

                StayOnPlayceEnemy enemy = Instantiate(_solderPrefab, position, rotation, transform);
                enemy.SetGroup(group);
                enemy.InitData(new UnitData(config.GetRandomUnitLevel), solderLevelsDatabase);
                
                _units.Add(enemy);
            }
        }

        public override void OnRestart()
        {
            base.OnRestart();
            _destroyable.transform.parent = transform;
        }

        protected override void Die()
        {
            foreach (var unit in _units)
            {
                unit.transform.parent = transform.parent;
            }

            _destroyable.transform.parent = transform.parent;
            _destroyable.DestroyObject();

            gameObject.SetActive(false);
        }

        protected override void OnGetDamage()
        {
           
        }
    }
}
