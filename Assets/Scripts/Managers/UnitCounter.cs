using System;
using units.Behavior;
using UnityEngine;

namespace Managers
{
    public class UnitCounter : MonoBehaviour
    {
        private UnitBaseBehaviour _unitBaseBehaviour;
        public static Action OnFriendlyCounterChanged;
        public static Action OnEnemyCounterChanged;

        public static int FriendlyCount { get; private set; }
        public static int EnemyCount { get; private set; }
        
        private void Awake()
        {
            _unitBaseBehaviour = GetComponent<UnitBaseBehaviour>();
            
        }
        private void Start()
        {
            if (_unitBaseBehaviour.Unit.IsFriendly)
            {
                FriendlyCount += _unitBaseBehaviour.Unit.Count;
                OnFriendlyCounterChanged?.Invoke();
            }
            else
            {
                EnemyCount++;
                OnEnemyCounterChanged?.Invoke();
            }
        }

        private void OnDestroy()
        {
            if (_unitBaseBehaviour == null || _unitBaseBehaviour.Unit == null) return;
            if (_unitBaseBehaviour.Unit.IsFriendly)
            {
                FriendlyCount -= _unitBaseBehaviour.Unit.Count;
                OnFriendlyCounterChanged?.Invoke();
            }
            else
            {
                EnemyCount--;
                OnEnemyCounterChanged?.Invoke();
            }
        }
    }
}