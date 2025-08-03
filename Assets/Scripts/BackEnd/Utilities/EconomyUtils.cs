using UnityEngine;

namespace BackEnd.Utilities
{
    public static class EconomyUtils
    {
        public static int ValidateAmount(int amount, string operation)
        {
            if (amount >= 0) return amount;
            Debug.LogWarning($"{amount} is negative. Please use a positive amount for {operation}.");
            return 0;
        }

        public static bool IsBelowHalf(int current, int max)
        {
            return (float)current / max < 0.5f;
        }

        public static bool IsAboveHalf(int current, int max)
        {
            return (float)current / max > 0.5f;
        }

        public static void DisplayHealthInConsole(int current, int max, string label = "Health")
        {
            Debug.Log($"{label} — Current: {current}, Max: {max}");
        }
    }
}