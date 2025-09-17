using System;
using BackEnd.Base_Classes;

namespace Managers
{
    public class GlobalUnitCounter : Singleton<GlobalUnitCounter>
    {
        public Action OnCountChanged;
        public int FriendlyCount { get; private set; }
        public int EnemyCount { get; private set; }

        public void AddCount(int count, bool friendly)
        {
            if (friendly)
            {
               FriendlyCount += count;
            }
            else
            { 
                EnemyCount  += count;
            }
            OnCountChanged?.Invoke();
        }
        public void SubtractCount(int count, bool friendly)
        {
            if (friendly)
            {
                FriendlyCount -= count;
            }
            else
            { 
                EnemyCount  -= count;
            }
            OnCountChanged?.Invoke();
        }
        
        public void ResetCounts()
        {
            FriendlyCount = 0;
            EnemyCount = 0;
            OnCountChanged?.Invoke();
        }
        
        
        
    }
}