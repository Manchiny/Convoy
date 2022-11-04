using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Restartable))]
    public abstract class Damageable : MonoBehaviour, IRestartable
    {
        public event Action<Damageable> Died;
        public event Action HealthChanged;

        public enum Team
        {
            Player,
            Enemy,
            Default
        }

        public abstract Team TeamId{ get; }

        public abstract int MaxHealth { get; protected set; }
        public abstract int Armor { get; }
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

            if (IsAlive == false)
            {
                Die();
                Died?.Invoke(this);
            }
            else
                HealthChanged?.Invoke();
        }

        public virtual void OnRestart()
        {
            ResetHealth();
        }

        public void AddHealth(int count)
        {
            if (IsAlive == false)
                return;

            Debug.Log($"{name} health befor adding {CurrentHealth}");

            if (CurrentHealth + count > MaxHealth)
                CurrentHealth = MaxHealth;
            else
                CurrentHealth += count;

            Debug.Log($"{name} health after adding: {CurrentHealth}");
            HealthChanged?.Invoke();
        }

        protected void ResetHealth()
        {
            CurrentHealth = MaxHealth;
            HealthChanged?.Invoke();
        }

        protected abstract void Die();
        protected abstract void OnGetDamage();
    }
}
