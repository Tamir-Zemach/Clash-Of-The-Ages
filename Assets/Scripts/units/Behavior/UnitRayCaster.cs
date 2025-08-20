using System;
using BackEnd.Data__ScriptableOBj_;
using Managers;
using UnityEngine;

namespace units.Behavior
{
    public class UnitRayCaster :  MonoBehaviour
    {
        public event Action<GameObject> OnFriendlyDetection;
        public event Action<GameObject> OnEnemyDetection;
        
        private UnitBaseBehaviour _unitBaseBehaviour;
        private UnitData _unit;
        private void Awake()
        {
            _unitBaseBehaviour  = GetComponent<UnitBaseBehaviour>();
            _unitBaseBehaviour.OnInitialized += GetUnitData;

        }

        private void OnDestroy()
        { 
            _unitBaseBehaviour.OnInitialized -= GetUnitData;
        }

        private void GetUnitData()
        {
            _unit = _unitBaseBehaviour.Unit;
        }

        public event Action OnNoDetection;

        private bool _wasDetectingSomething;

        private void Update()
        {
            
            if (!GameStates.Instance.GameIsPlaying) return;

            bool detected = false;

            detected |= CheckForUnitInFront(_unit.FriendlyUnitMask, _unit.RayLengthForFriendlyUnit, FriendlyDetection, _unit.FriendlyUnitTag);
            detected |= CheckForUnitInFront(_unit.OppositeUnitMask, _unit.Range, EnemyDetection, _unit.OppositeUnitTag, _unit.OppositeBaseTag);
            
            if (!detected && _wasDetectingSomething)
            {
                OnNoDetection?.Invoke();
            }

            _wasDetectingSomething = detected;
        }
        
        
        
        private bool CheckForUnitInFront(LayerMask mask, float range, Action<GameObject> onDetected, string unitTag, string baseTag = null)
        {

            if (Physics.BoxCast(transform.position, _unit.boxSize, transform.forward,
                    out var hitInfo, Quaternion.identity, range, mask))
            {
                GameObject obj = hitInfo.transform.gameObject;

                if (obj.CompareTag(unitTag) || (baseTag != null && obj.CompareTag(baseTag)))
                {
                    onDetected?.Invoke(obj);
                    return true;
                }
            }

            return false;
        }

        private void FriendlyDetection(GameObject obj)
        {
            OnFriendlyDetection?.Invoke(obj);
        }
        
        private void EnemyDetection(GameObject obj)
        {
            OnEnemyDetection?.Invoke(obj);
        }
        
    }
}