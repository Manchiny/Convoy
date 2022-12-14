using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : Unit
    {
        [SerializeField] private Transform _badgeHolder;

        private bool _inited;

        private HashSet<SolderBadge> _badges = new();

        public override Team TeamId => Team.Player;

        public UnitData GetData => Data;

        public PlayerMovement Movement { get; private set; }
        public bool InTankZone { get; private set; } = true;

        private void Awake()
        {
            Movement = GetComponent<PlayerMovement>();
            Movement.OnMovementStarted += OnStartMovement;
            Movement.OnMovementStoped += OnStopMovement;
        }

        private void OnDestroy()
        {
            Movement.OnMovementStarted -= OnStartMovement;
            Movement.OnMovementStoped -= OnStopMovement;
            Data.Changed -= UpdateDataProperties;
        }

        private void Update()
        {
            if (_inited == false)
                return;

            if (IsAlive == false)
                return;

            if (Target != null)
                if (Target.gameObject.activeInHierarchy && Target.IsAlive == true)
                    TryShoot();
                else
                    RemoveFromEnemies(Target);
        }

        public void OnLevelStarted(Vector3 spawnPosition)
        {
            _inited = false;
            transform.position = spawnPosition;

            ResetHealth();
            InTankZone = true;

            // _badges.Clear();
            ClearTargets();

            _inited = true;
        }

        public void TakeBadge(SolderBadge badge)
        {
           badge.MoveToHolder(_badgeHolder, _badges.Count);
            _badges.Add(badge);
        }

        public void OnTankZoneLeave()
        {
            InTankZone = false;
        }

        public void OnTankZoneEntered()
        {
            InTankZone = true;
        }

        protected override void OnGetDamage()
        {
            //Debug.Log("Player take damage");
        }

        protected override void Die()
        {
            Debug.LogWarning("Game over");
        }

        protected override void OnDataInited()
        {
            Data.Changed += UpdateDataProperties;
        }

        protected override void OnEnemyFinded(Damageable enemy)
        {
            if (Movement.IsStoped && (Target == null || Target.IsAlive == false))
                OnStopMovement();
        }

        protected override void OnEnenmyMissed(Damageable enemy)
        {
            if (enemy == Target || (Target != null && Target.IsAlive == false))
                Target = TryGetNearestTarget();
        }

        private void OnStartMovement()
        {
            Target = null;
        }

        private void OnStopMovement()
        {
            Target = TryGetNearestTarget();
        }
    }
}
