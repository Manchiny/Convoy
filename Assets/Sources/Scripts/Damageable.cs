using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Restartable))]
    public abstract class Damageable : MonoBehaviour, IRestartable
    {
        public event Action<Damageable> Died;
        public event Action Damaged;

        public enum Team
        {
            Player,
            Enemy,
            Default
        }

        public abstract Team TeamId{ get; }

        public abstract int MaxHealth { get; protected set; }
        public abstract int Armor { get; protected set; }
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
                Damaged?.Invoke();
        }

        public virtual void OnRestart()
        {
            ResetHealth();
        }

        protected void ResetHealth()
        {
            CurrentHealth = MaxHealth;
        }

        protected abstract void Die();
        protected abstract void OnGetDamage();
    }
}
