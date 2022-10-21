using UnityEngine;

namespace Assets.Scripts.Units
{
    public class ShelterEnemy : Unit
    {
        private Tower _tower;
        public override int MaxHealth => 100;

        public override Team TeamId => Team.Enemy;

        private void Update()
        {
            if (IsAlive == false)
                return;

            if (Target != null)
            {
                RotateToTarget();
                TryShoot();
            }
        }

        public void ForceDie()
        {
            GetDamage(MaxHealth);
        }

        protected override void Die()
        {
            gameObject.SetActive(false);
        }

        protected override void OneEnenmyMissed(Damageable enemy)
        {
            if (enemy == Target || (Target != null && Target.IsAlive == false))
                Target = TryGetAnyNewTarget();
        }

        protected override void OnEnemyFinded(Damageable enemy)
        {
            if (Target == null)
                Target = enemy;

            else if (enemy is Tank && Target is not Tank)
            {
                Target = enemy;
                Debug.Log("Enemy find tank");
            }
        }

        protected override void OnGetDamage()
        {

        }

        private void RotateToTarget()
        {
            Vector3 lookPosition = Target.transform.position;
            lookPosition.y = transform.position.y;

            transform.LookAt(lookPosition);
        }
    }
}

