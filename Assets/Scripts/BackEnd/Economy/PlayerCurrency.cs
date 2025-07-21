using System;
using UnityEngine;

namespace BackEnd.Economy
{
    public class PlayerCurrency
    {
        public static event Action OnMoneyChanged;

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
            return _money >= cost;
        }

        public void DisplyMoneyInConsole()
        {
            Debug.Log($"current money amount = {_money}");
        }

    }
}
