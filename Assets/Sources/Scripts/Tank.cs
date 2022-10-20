using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Tank : Unit
    {
        private const float TowerRotationSpeed = 12f;
        private const float MoveSpeed = 2f;

        private const float DestinationDistance = 1f;

        private IReadOnlyList<Vector3> _waypoints;
        private Vector3 _currnentTargetPoint;
        private int _currentWaypointId;

        public override int MaxHealth => 200;
        public override Team TeamId => Team.Player;

        private bool _inited;

        private bool NeedRotateTower => Target != null && Target.IsAlive && CheckTowerDirection() == false;

        private void Update()
        {
            if (!_inited)
                return;

            if (Target != null)
            {
                if (NeedRotateTower)
                    RotateTower();
                else
                    TryShoot();
            }
            else
            {
                if ((_currnentTargetPoint - transform.position).sqrMagnitude < DestinationDistance * DestinationDistance)
                    OnWaypointReached();

                Rotate();
                MoveToNextPoint();
            }
        }

        public void Init(Vector3 spawnPosition, IReadOnlyList<Vector3> waypoints)
        {
            transform.position = spawnPosition;
            transform.rotation = Quaternion.identity;

            _waypoints = waypoints;
            _currentWaypointId = 0;

            _currnentTargetPoint = waypoints[0];

            _inited = true;
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

        protected override void OneEnenmyMissed(Damageable enemy)
        {
            if (enemy == Target || (Target != null && Target.IsAlive == false))
                Target = TryGetAnyNewTarget();
        }

        private void SetTarget(Damageable enemy)
        {
            Target = enemy;
        }

        private void MoveToNextPoint()
        {
            transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }

        private void Rotate()
        {
            Vector3 lookPos = Vector3.zero;

            lookPos = _currnentTargetPoint - transform.position;

            lookPos.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 50f * Time.deltaTime);
        }

        private void OnWaypointReached()
        {
            if (_currentWaypointId < _waypoints.Count - 1)
            {
                _currentWaypointId++;
                _currnentTargetPoint = _waypoints[_currentWaypointId];
            }
            else
            {
                //level win;
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

        private void RotateTower()
        {
            Vector3 lookPos = Vector3.zero;

            lookPos = Target.transform.position - Gun.transform.position;

            lookPos.y = 0;

            Quaternion towerRotation = Quaternion.LookRotation(lookPos);
            Gun.transform.rotation = Quaternion.RotateTowards(Gun.transform.rotation, towerRotation, TowerRotationSpeed * Time.deltaTime);
        }
    }
}
