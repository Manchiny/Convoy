using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : Unit
    {
        [SerializeField] private Transform _badgeHolder;

        private HashSet<SolderBadge> _badges = new();

        public override int MaxHealth => 10000;
        public override Team TeamId => Team.Player;

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
        }

        private void Update()
        {
            if (IsAlive == false)
                return;

            if (Target != null)
                if (Target.IsAlive)
                    TryShoot();
                else
                    RemoveFromEnemies(Target);
        }

        public void TakeBadge(SolderBadge badge)
        {
           badge. MoveToHolder(_badgeHolder, _badges.Count);
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

        protected override void OnEnemyFinded(Damageable enemy)
        {
           
        }

        protected override void OneEnenmyMissed(Damageable enemy)
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
