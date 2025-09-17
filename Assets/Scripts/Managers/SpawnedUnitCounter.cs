using System;
using units.Behavior;
using UnityEngine;

namespace Managers
{
    public class SpawnedUnitCounter : MonoBehaviour
    {
        private UnitBaseBehaviour _unitBaseBehaviour;
        private UnitHealthManager _unitHealthManager;
        
        private void Awake()
        {
            _unitBaseBehaviour = GetComponent<UnitBaseBehaviour>();
            _unitHealthManager =  GetComponent<UnitHealthManager>();
            
        }
        private void Start()
        {
            _unitHealthManager.OnDying += SubtractCount;
            if (!_unitBaseBehaviour.Unit.IsFriendly)
            {
                GlobalUnitCounter.Instance.AddCount(1, friendly: false);
            }
        }

        
        private void SubtractCount()
        {
            GlobalUnitCounter.Instance.SubtractCount(1, friendly: _unitBaseBehaviour.Unit.IsFriendly);
        }

        private void OnDestroy()
        {
            _unitHealthManager.OnDying -= SubtractCount;
        }
    }
}