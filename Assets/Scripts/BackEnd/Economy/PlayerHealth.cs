using System;
using UnityEngine;

namespace BackEnd.Economy
{
    internal class PlayerHealth
    {
        public static event Action OnHealthChanged;
        public static event Action OnDroppedBelowHalfHealth;
        public static event Action OnHealedAboveHalfHealth;

        private bool _hasDroppedBelowHalfHealth = false;

        private static PlayerHealth _instance;

        // Public property to access the instance
        public static PlayerHealth Instance => _instance ??= new PlayerHealth();

        // Private constructor to prevent external instantiation
        private PlayerHealth() { }

        private int _currentHealth = 1;
        private int _maxHealth = 2;
        public int CurrentHealth => _currentHealth; // Read-only property
        public int MaxHealth => _maxHealth; // Read-only property

        public int AddHealth(int amount)
        {
            _currentHealth += ValidateAmount(Math.Max(0, amount), "adding");
            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
                OnHealthChanged?.Invoke();
                EvaluateHealthThresholds();
                return _currentHealth;
            }
            OnHealthChanged?.Invoke();
            EvaluateHealthThresholds();
            return _currentHealth;
        }

        public int SubtractHealth(int amount)
        {
            _currentHealth -= ValidateAmount(Math.Max(0, amount), "subtracting");
            OnHealthChanged?.Invoke();
            EvaluateHealthThresholds();
            return _currentHealth;
        }
        public int IncreaseMaxHealth(int amount)
        {
            _maxHealth += ValidateAmount(Math.Max(0, amount), "adding");
            OnHealthChanged?.Invoke();
            return _maxHealth;
        }

        public int SetMaxHealth(int amount)
        {
            _maxHealth = amount;
            OnHealthChanged?.Invoke();
            return _maxHealth;
        }

        public void FullHealth()
        {
            _currentHealth = _maxHealth;
            OnHealthChanged?.Invoke();
            EvaluateHealthThresholds();
        }

        private int ValidateAmount(int amount, string operation)
        {
            if (amount >= 0) return amount;
            Debug.LogWarning($"{amount} is negative. Please use a positive amount for {operation}.");
            return 0;
        }

        private void EvaluateHealthThresholds()
        {
            if (!_hasDroppedBelowHalfHealth && (float)_currentHealth / _maxHealth < 0.5f)
            {
                _hasDroppedBelowHalfHealth = true;
                OnDroppedBelowHalfHealth?.Invoke();
            }

            if (!_hasDroppedBelowHalfHealth || !((float)_currentHealth / _maxHealth > 0.5f)) return;
            _hasDroppedBelowHalfHealth = false;
            OnHealedAboveHalfHealth?.Invoke();
        }


        public bool PlayerDied()
        {
            return _currentHealth <= 0; 
        }
        public void DisplyHealthInConsole()
        {
            Debug.Log($"Current health amount = {_currentHealth}, Max health amount = {_maxHealth}");
        }
        public void ResetHealthFlags()
        {
            _hasDroppedBelowHalfHealth = false;
        }

    }
}
