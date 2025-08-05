using System;
using BackEnd.Base_Classes;
using BackEnd.Utilities;
using UnityEngine;

namespace BackEnd.Economy
{
    internal class EnemyHealth : OneInstanceClass<EnemyHealth>
    {
        public static event Action OnEnemyHealthChanged;
        public event Action OnEnemyDied;
        public static event Action OnDroppedBelowHalfHealth;

        private bool _hasDroppedBelowHalfHealth = false;

        private int _currentHealth = 1;
        private int _maxHealth = 2;
        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;

        public int AddHealth(int amount)
        {
            _currentHealth += EconomyUtils.ValidateAmount(Math.Max(0, amount), "adding");
            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
            OnEnemyHealthChanged?.Invoke();
            EvaluateHealthThresholds();
            return _currentHealth;
        }

        public int SubtractHealth(int amount)
        {
            _currentHealth -= EconomyUtils.ValidateAmount(Math.Max(0, amount), "subtracting");
            EvaluateHealthThresholds();
            OnEnemyHealthChanged?.Invoke();

            if (_currentHealth <= 0)
            {
                OnEnemyDied?.Invoke();
            }

            return _currentHealth;
        }

        public int IncreaseMaxHealth(int amount)
        {
            _maxHealth += EconomyUtils.ValidateAmount(Math.Max(0, amount), "adding");
            OnEnemyHealthChanged?.Invoke();
            return _maxHealth;
        }

        public int SetMaxHealth(int amount)
        {
            _maxHealth = amount;
            OnEnemyHealthChanged?.Invoke();
            return _maxHealth;
        }

        public void FullHealth()
        {
            _currentHealth = _maxHealth;
            EvaluateHealthThresholds();
            OnEnemyHealthChanged?.Invoke();
        }

        private void EvaluateHealthThresholds()
        {
            if (!_hasDroppedBelowHalfHealth && EconomyUtils.IsBelowHalf(_currentHealth, _maxHealth))
            {
                _hasDroppedBelowHalfHealth = true;
                OnDroppedBelowHalfHealth?.Invoke();
            }

            if (_hasDroppedBelowHalfHealth && EconomyUtils.IsAboveHalf(_currentHealth, _maxHealth))
            {
                _hasDroppedBelowHalfHealth = false;
            }
        }

        public void DisplayHealthInConsole()
        {
            EconomyUtils.DisplayHealthInConsole(_currentHealth, _maxHealth, "Enemy Health");
        }
    }
}