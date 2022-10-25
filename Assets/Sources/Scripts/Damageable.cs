using System;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Damageable : MonoBehaviour
    {
        public event Action<Damageable> Died;
        public event Action Damaged;

        public enum Team
        {
            Player,
            Enemy,
            Default
        }

        public abstract int MaxHealth { get; }
        public abstract int Armor { get; }
        public abstract Team TeamId{ get; }
        public int CurrentHealth { get; protected set; }
        public bool IsAlive => CurrentHealth > 0;

        protected virtual void Start()
        {
            ResetHealth();
        }

        public void GetDamage(int damage)
        {
            if (damage <= 0 || IsAlive == false)
                return;

            int totalDamage = damage - Armor;

            if (totalDamage < 0)
                totalDamage = 0;

            CurrentHealth -= totalDamage;
            OnGetDamage();

            Debug.Log($"{name} take damage: {totalDamage}; Health = {CurrentHealth}");

            if (IsAlive == false)
            {
                Die();
                Died?.Invoke(this);
            }
            else
                Damaged?.Invoke();
        }

        protected abstract void Die();
        protected abstract void OnGetDamage();

        protected void ResetHealth()
        {
            CurrentHealth = MaxHealth;
        }
    }
}
