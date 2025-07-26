using System;
using UnityEngine;

namespace BackEnd.Economy
{
    public class PlayerCurrency
    {
        public static event Action OnMoneyChanged;
        public static event Action OnDoesntHaveEnoughMoney;

        private static PlayerCurrency instance;

        // Public property to access the instance
        public static PlayerCurrency Instance => instance ??= new PlayerCurrency();

        // Private constructor to prevent external instantiation
        private PlayerCurrency() { }

        private int _money = 0;
        public int Money => _money; // Read-only property

        public int AddMoney(int amount)
        {
            _money += ValidateAmount(Math.Max(0, amount), "adding");
            OnMoneyChanged?.Invoke();
            return _money;
        }

        public int SubtractMoney(int amount)
        {
            _money -= ValidateAmount(Math.Max(0, amount), "subtracting");
            if (_money < 0)
            {
                _money = 0;
            }
            OnMoneyChanged?.Invoke();
            return _money;
        }

        
        private int ValidateAmount(int amount, string operation)
        {
            if (amount < 0)
            {
                Debug.LogWarning($"{amount} is negative. Please use a positive amount for {operation}.");
                return 0;
            }
            return amount;
        }

        public bool HasEnoughMoney(int cost)
        {
            if (_money >= cost)
            {
                return true;
            }
            else
            {
                OnDoesntHaveEnoughMoney?.Invoke();
                return false;
            }
        }
    }
}
