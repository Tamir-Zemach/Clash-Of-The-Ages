using System;
using BackEnd.Base_Classes;
using BackEnd.Utilities;
using Managers;
using UnityEngine;

namespace BackEnd.Economy
{
    internal class PlayerHealth : OneInstanceClass<PlayerHealth>
    {
        public static event Action OnHealthChanged;
        public static event Action OnDroppedBelowHalfHealth;
        public static event Action OnHealedAboveHalfHealth;

        private bool _hasDroppedBelowHalfHealth = false;

        private int _currentHealth = 1;
        private int _maxHealth = 2;
        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;

        public int AddHealth(int amount)
        {
            _currentHealth += EconomyUtils.ValidateAmount(Math.Max(0, amount), "adding");
            if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;
            OnHealthChanged?.Invoke();
            EvaluateHealthThresholds();
            return _currentHealth;
        }

        public int SubtractHealth(int amount)
        {
            _currentHealth -= EconomyUtils.ValidateAmount(Math.Max(0, amount), "subtracting");
            OnHealthChanged?.Invoke();
            EvaluateHealthThresholds();
            if (_currentHealth <= 0)
            {
                GameStates.Instance.EndGame();
            }
            return _currentHealth;
        }

        public int IncreaseMaxHealth(int amount)
        {
            _maxHealth += EconomyUtils.ValidateAmount(Math.Max(0, amount), "adding");
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
                OnHealedAboveHalfHealth?.Invoke();
            }
        }

        public void DisplyHealthInConsole()
        {
            EconomyUtils.DisplayHealthInConsole(_currentHealth, _maxHealth, "Player Health");
        }

        public void ResetHealthFlags()
        {
            _hasDroppedBelowHalfHealth = false;
        }
    }
}