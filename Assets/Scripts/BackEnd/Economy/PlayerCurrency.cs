using System;
using BackEnd.Base_Classes;
using BackEnd.Utilities;
using UnityEngine;

namespace BackEnd.Economy
{
    public class PlayerCurrency : Singleton<PlayerCurrency>
    {
        public static event Action OnMoneyChanged;
        public static event Action OnDoesntHaveEnoughMoney;
        
        private int _money = 0;
        public int Money => _money;

        public int AddMoney(int amount)
        {
            _money += EconomyUtils.ValidateAmount(Math.Max(0, amount));
            OnMoneyChanged?.Invoke();
            return _money;
        }

        public int SetMoney(int amount)
        {
            _money = EconomyUtils.ValidateAmount(Math.Max(0, amount));
            OnMoneyChanged?.Invoke();
            return _money;
        }

        public int SubtractMoney(int amount)
        {
            _money -= EconomyUtils.ValidateAmount(Math.Max(0, amount));
            if (_money < 0) _money = 0;
            OnMoneyChanged?.Invoke();
            return _money;
        }

        public bool HasEnoughMoney(int cost)
        {
            if (_money >= cost) return true;
            OnDoesntHaveEnoughMoney?.Invoke();
            return false;
        }
    }
}