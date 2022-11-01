using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(UnitBoosts))]
    public class Tank : Unit, IBoostable
    {
        private const float MoveSpeed = 2f;
        private const float RotationSpeed = 50f;
        private const float DestinationDistance = 1f;

        private const float TowerRotationSpeed = 12f;

        private UnitBoosts _boosts;

        private IReadOnlyList<Vector3> _waypoints;
        private Vector3 _currnentTargetPoint;
        private int _currentWaypointId;

        private bool _inited;
        private bool _completed;

        public event Action Completed;

        public override Team TeamId => Team.Player;
        private bool NeedRotateTower => Target != null && Target.IsAlive && CheckTowerDirection() == false;

        public override int Damage => _boosts.TryGetBoostValue(ItemType.DamageMultyplier, out float value) ? base.Damage * (int)value : base.Damage;
        public override int Armor => _boosts.TryGetBoostValue(ItemType.ArmorMultyplier, out float value) ? base.Armor * (int)value : base.Armor;
        public override float ShootDelay => _boosts.TryGetBoostValue(ItemType.ShootingDelayDivider, out float value) ? base.ShootDelay / value : base.ShootDelay;

        private void Update()
        {
            if (!_inited || Data == null || IsAlive == false || _completed)
                return;

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

        private void OnDestroy()
        {
            Data.Changed -= UpdateDataProperties;
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

        public void InitUnitBoosts(UnitBoosts boosts)
        {
            _boosts = boosts;
        }

        public void AddBoost(ItemType type, float value)
        {
            _boosts.TryAddBoost(type, value);
        }

        public void RemoveBoost(ItemType type)
        {
            _boosts.RemoveBoost(type);
        }

        protected override void Die() { }

        protected override void OnGetDamage(){}

        protected override void OnDataInited()
        {
            Data.Changed += UpdateDataProperties;
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
