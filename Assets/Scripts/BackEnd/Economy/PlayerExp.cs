using System;
using BackEnd.Base_Classes;
using BackEnd.Utilities;
using UnityEngine;

namespace BackEnd.Economy
{
    public class PlayerExp : OneInstanceClass<PlayerExp>
    {
        public static event Action OnExpChanged;
        public static event Action OnExpToLevelUpChanged;
        public event Action OnLevelUp;

        private int _currentExp = 0;
        private int _expToLevelUp = 100;
        private int _level = 1;

        public int CurrentExp => _currentExp;
        public int ExpToLevelUp => _expToLevelUp;
        public int Level => _level;

        public int AddExp(int amount)
        {
            _currentExp += EconomyUtils.ValidateAmount(Math.Max(0, amount), "adding EXP");
            OnExpChanged?.Invoke();

            while (_currentExp >= _expToLevelUp)
            {
                _currentExp -= _expToLevelUp;
                LevelUp();
            }

            return _currentExp;
        }

        public int SetExp(int amount)
        {
            _currentExp = EconomyUtils.ValidateAmount(Math.Max(0, amount), "setting EXP");
            OnExpChanged?.Invoke();
            return _currentExp;
        }

        public void SetExpToLevelUp(int amount)
        {
            _expToLevelUp = EconomyUtils.ValidateAmount(Math.Max(1, amount), "setting EXP threshold");
            OnExpToLevelUpChanged?.Invoke();
        }

        public void SetLevel(int newLevel)
        {
            _level = EconomyUtils.ValidateAmount(Math.Max(1, newLevel), "setting level");
            OnExpChanged?.Invoke();
        }

        public void LevelUp()
        {
            _level++;
            OnLevelUp?.Invoke();
        }

        public void DisplayExpInConsole()
        {
            Debug.Log($"EXP: {_currentExp}/{_expToLevelUp}, Level: {_level}");
        }

        public void ResetExp()
        {
            _currentExp = 0;
            _level = 1;
            OnExpChanged?.Invoke();
        }
    }
}