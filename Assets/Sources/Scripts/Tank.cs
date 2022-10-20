using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public class Tank : Unit
    {
        private const float TowerRotationSpeed = 12f;

        private List<Vector3> _waypoints;
        private Vector3 _currnentTargetPoint;
        private int _currentWaypointId;

        public override int MaxHealth => 200;
        public override Team TeamId => Team.Player;

        private bool _inited;

        private bool NeedRotateTower => Target != null && Target.IsAlive && CheckTowerDirection() == false;

        private void Update()
        {
            //if (!_inited)
            //    return;

            if (Target != null)
                if (NeedRotateTower)
                    RotateTower();
                else
                    TryShoot();

        }

        public void Init(Vector3 spawnPosition, List<Vector3> waypoints)
        {
            transform.position = spawnPosition;
            transform.rotation = Quaternion.identity;

            _waypoints = waypoints;
            _currentWaypointId = 0;

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
