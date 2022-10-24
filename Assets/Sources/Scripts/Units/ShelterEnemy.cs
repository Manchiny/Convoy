using UnityEngine;

namespace Assets.Scripts.Units
{
    [RequireComponent(typeof(Rigidbody))]
    public class ShelterEnemy : Enemy
    {
        private Rigidbody _rigidbody;

        public override int MaxHealth => 100;
        public override Team TeamId => Team.Enemy;
        public override int Damage => 15;
        public override int Armor => 0;

        protected override void Awake()
        {
            base.Awake();
            _rigidbody = GetComponent<Rigidbody>();
        }

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

        protected override void OnDie() 
        {
            _rigidbody.isKinematic = false;
        }

        protected override void OnGetDamage() { }

        private void RotateToTarget()
        {
            Vector3 lookPosition = Target.transform.position;
            lookPosition.y = transform.position.y;

            transform.LookAt(lookPosition);
        }


    }
}

