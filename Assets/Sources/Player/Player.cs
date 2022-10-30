using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(UnitBoosts))]
    public class Player : Unit, IBoostable
    {
        [SerializeField] private Transform _badgeHolder;

        private UnitBoosts _boosts;
        private bool _inited;

        private HashSet<SolderBadge> _badges = new();

        public event Action<int> BadgesChanged;

        public override Team TeamId => Team.Player;

        public UnitData GetData => Data;

        public PlayerMovement Movement { get; private set; }
        public bool InTankZone { get; private set; } = true;

        public override int Damage => _boosts.TryGetBoostValue(ItemType.DamageMultyplier, out float value) ? base.Damage * (int)value : base.Damage;
        public override int Armor => _boosts.TryGetBoostValue(ItemType.ArmorMultyplier, out float value) ? base.Armor * (int)value : base.Armor;
        public override float ShootDelay => _boosts.TryGetBoostValue(ItemType.ShootingDelayDivider, out float value) ? base.ShootDelay / value : base.ShootDelay;

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

            foreach (var badge in _badges)
                Destroy(badge.gameObject);

             _badges.Clear();
            BadgesChanged?.Invoke(_badges.Count);

            ClearTargets();

            _inited = true;
        }

        public void TakeBadge(SolderBadge badge)
        {
            badge.MoveToHolder(_badgeHolder, _badges.Count);
            _badges.Add(badge);
            BadgesChanged?.Invoke(_badges.Count);
        }

        public void OnTankZoneLeave() { InTankZone = false; }
        public void OnTankZoneEntered() { InTankZone = true; }
        public void InitUnitBoosts(UnitBoosts boosts) { _boosts = boosts; }

        public void AddBoost(ItemType type, float value) 
        {
            _boosts.TryAddBoost(type, value);
        }

        public void RemoveBoost(ItemType type)
        {
            _boosts.RemoveBoost(type);
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
