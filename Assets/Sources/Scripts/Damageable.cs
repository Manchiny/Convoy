using System;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Damageable : MonoBehaviour
    {
        public event Action<Damageable> Died;

        public abstract int MaxHealth { get; }
        public int CurrentHealth { get; protected set; }
        public bool IsAlive => CurrentHealth > 0;

        protected virtual void Start()
        {
            Init();
        }

        public void GetDamage(int damage)
        {
            if (damage <= 0 || IsAlive == false)
                return;

            CurrentHealth -= damage;
            OnGetDamage();

            Debug.Log($"{name} take damage: {damage}; Health = {CurrentHealth}");

            if (IsAlive == false)
            {
                Die();
                Died?.Invoke(this);
            }
        }

        protected abstract void Die();
        protected abstract void OnGetDamage();

        private void Init()
        {
            CurrentHealth = MaxHealth;
        }
    }
}
