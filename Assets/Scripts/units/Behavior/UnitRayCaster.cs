using System;
using BackEnd.Data__ScriptableOBj_;
using Managers;
using UnityEngine;

namespace units.Behavior
{
    public class UnitRayCaster : MonoBehaviour
    {
        public event Action<GameObject> OnFriendlyDetection;
        public event Action<GameObject> OnEnemyDetection;
        public event Action OnNoDetection;

        private UnitBaseBehaviour _unitBaseBehaviour;
        private UnitData _unit;
        private bool _wasDetectingSomething;
        public bool IsDetecting => _wasDetectingSomething;
        private void Awake()
        {
            _unitBaseBehaviour = GetComponent<UnitBaseBehaviour>();
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

        private void Update()
        {
            if (_unit == null || !GameStates.Instance.GameIsPlaying) return;

            bool detected = false;

            detected |= CheckForUnitsInFront(_unit.FriendlyUnitMask, _unit.RayLengthForFriendlyUnit, FriendlyDetection, _unit.FriendlyUnitTag);
            detected |= CheckForUnitsInFront(_unit.OppositeUnitMask, _unit.Range, EnemyDetection, _unit.OppositeUnitTag, _unit.OppositeBaseTag);

            if (!detected && _wasDetectingSomething)
            { 
                OnNoDetection?.Invoke();
            }

            _wasDetectingSomething = detected;
        }

        private bool CheckForUnitsInFront(LayerMask mask, float range, Action<GameObject> onDetected, string unitTag, string baseTag = null)
        {
            Vector3 origin = transform.position - transform.forward * (_unit.boxSize.z * 0.5f);
            RaycastHit[] hits = Physics.BoxCastAll(origin, _unit.boxSize, transform.forward, Quaternion.identity, range, mask);

            foreach (var hit in hits)
            {
                GameObject obj = hit.transform.gameObject;

                if (obj == gameObject || obj == transform.root.gameObject)
                    continue;
                

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