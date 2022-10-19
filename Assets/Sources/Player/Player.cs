using UnityEngine;

namespace Assets.Scripts.Characters
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : Character
    {
        public PlayerMovement Movement { get; private set; }

        public override int MaxHealth => 10000;

        public override Team TeamId => Team.Player;

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
            if (Target != null)
                if (Target.IsAlive)
                    TryShoot();
                else
                    RemoveFromEnemies(Target);
        }

        protected override void OnGetDamage()
        {
            Debug.Log("Player take damage");
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
