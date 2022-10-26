using System;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.UnitData;

namespace Assets.Scripts.Units
{
    public class Tank : Unit
    {
        private const float MoveSpeed = 2f;
        private const float RotationSpeed = 50f;
        private const float DestinationDistance = 1f;

        private const float TowerRotationSpeed = 12f;

        private IReadOnlyList<Vector3> _waypoints;
        private Vector3 _currnentTargetPoint;
        private int _currentWaypointId;

        private bool _inited;
        private UnitData _data;
        private UnitPropertiesDatabase _propertiesDatabase;
        private bool _completed;

        public event Action Completed;

        public override Team TeamId => Team.Player;

        public override int MaxHealth => 15000;
        public override int Armor => _data.GetArmor(_propertiesDatabase);
        public override int Damage => _data.GetDamage(_propertiesDatabase);
        public override float ShootDelay => _data.GetShootDelay(_propertiesDatabase);

        public UnitData GetData => _data;

        private bool NeedRotateTower => Target != null && Target.IsAlive && CheckTowerDirection() == false;

        private void Update()
        {
            if (!_inited || _data == null || IsAlive == false || _completed)
                return;

#if UNITY_EDITOR
            if (Input.GetKeyUp(KeyCode.Space) == true)
            {
                _data.AddUpgradePoint(StatName.Damage);
                Game.Instance.Save();
            }
#endif

            if (Target != null)
            {
                if (NeedRotateTower)
                    Rotate(Gun.transform, Target.transform.position, TowerRotationSpeed);
                else
                    TryShoot();
            }
            else
            {
                if ((_currnentTargetPoint - transform.position).sqrMagnitude < DestinationDistance * DestinationDistance)
                    OnWaypointReached();

                Rotate(transform, _currnentTargetPoint, RotationSpeed);
                Move();
                RotateTowerToZero();
            }
        }

        public void Init(UnitData data, UnitPropertiesDatabase propertiesDatabase)
        {
            _data = data;
            _propertiesDatabase = propertiesDatabase;
        }

        public void OnLevelStarted(Vector3 spawnPosition, IReadOnlyList<Vector3> waypoints)
        {
            _inited = false;

            transform.position = spawnPosition;
            transform.rotation = Quaternion.identity;

            _waypoints = waypoints;
            _currentWaypointId = 0;

            _currnentTargetPoint = waypoints[0];

            ResetHealth();
            ClearTargets();

            _inited = true;
            _completed = false;
        }

        public void OnComplete()
        {
            Debug.Log("[Tank] completed;");
            _completed = true;
            Completed?.Invoke();
        }

        protected override void Die()
        {

        }

        protected override void OnGetDamage()
        {

        }

        protected override void OnEnemyFinded(Damageable enemy)
        {
            if (Target == null)
                SetTarget(enemy);
        }

        protected override void OnEnenmyMissed(Damageable enemy)
        {
            if (enemy == Target || (Target != null && Target.IsAlive == false))
                Target = TryGetAnyNewTarget();
        }

        private void SetTarget(Damageable enemy)
        {
            Target = enemy;
        }

        private void Move()
        {
            transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }

        private void OnWaypointReached()
        {
            if (_currentWaypointId < _waypoints.Count - 1)
            {
                _currentWaypointId++;
                _currnentTargetPoint = _waypoints[_currentWaypointId];
            }
        }

        private bool CheckTowerDirection()
        {
            Vector3 towerPoition = Gun.transform.position;
            towerPoition.y = 0;

            Vector3 targetPosition = Target.transform.position;
            targetPosition.y = 0;

            Vector3 enemyDirection = (targetPosition - towerPoition).normalized;

            return enemyDirection == Gun.transform.forward.normalized;
        }

        private void Rotate(Transform rotatedTransform, Vector3 lookAtPosition, float speed)
        {
            Vector3 lookPos = Vector3.zero;

            lookPos = lookAtPosition - rotatedTransform.position;
            lookPos.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPos);
            rotatedTransform.rotation = Quaternion.RotateTowards(rotatedTransform.rotation, rotation, speed * Time.deltaTime);
        }

        private void RotateTowerToZero()
        {
            Quaternion rotation = Quaternion.identity;
            Gun.transform.localRotation = Quaternion.RotateTowards(Gun.transform.localRotation, rotation, TowerRotationSpeed * Time.deltaTime);
        }
    }
}
