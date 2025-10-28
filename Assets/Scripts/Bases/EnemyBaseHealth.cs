using System;
using BackEnd.Economy;
using BackEnd.InterFaces;
using BackEnd.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Bases
{
    public class EnemyBaseHealth : MonoBehaviour, IDamageable
    {
        public Action OnBaseDestroyed; 
        public Action OnHealthChanged; 
        public Action<int> OnMaxHealthChanged;
        public UnityEvent OnBaseDestroyedForEffects;
        
        private int _maxHealth;
        private int _currentHealth;
        
        public int CurrentHealth => _currentHealth;
        
        public void GetHurt(int damage)
        {
            if (_currentHealth > 0)
            {
                EnemyHealth.Instance.SubtractHealth(damage);
            }
            SubtractLocalHealth(damage);
        }

        private void SubtractLocalHealth(int damage)
        {
            _currentHealth -= EconomyUtils.ValidateAmount(Math.Max(0, damage));
            OnHealthChanged?.Invoke();
            if (_currentHealth <= 0)
            {
                OnBaseDestroyed?.Invoke();
                OnBaseDestroyedForEffects?.Invoke();
            }
        }

        public void SetLocalHealth(int health)
        {
            _maxHealth = health;
            _currentHealth = _maxHealth;
            OnMaxHealthChanged?.Invoke(_maxHealth);
            print($"base health: {health}, max health: {EnemyHealth.Instance.MaxHealth}");
        }
        
        
    }
}