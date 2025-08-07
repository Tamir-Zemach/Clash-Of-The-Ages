using System;
using BackEnd.Data__ScriptableOBj_;
using units.Behavior;
using UnityEngine;



namespace units.Behavior
{
    [RequireComponent(typeof(UnitBaseBehaviour))]
    public class UnitDebugger : MonoBehaviour
    {
        private UnitBaseBehaviour _unitBaseBehaviour;
        private UnitData  _unit;

        private void Awake()
        {
            _unitBaseBehaviour = GetComponent<UnitBaseBehaviour>();
            _unit = _unitBaseBehaviour.Unit;
        }


        private void OnDrawGizmos()
        {
            if (_unit == null) return;
            Gizmos.color = _unit.boxColor;

            Vector3 origin = transform.position;
            Vector3 direction = transform.forward;

            // Perform the BoxCast
            if (Physics.BoxCast(origin,_unit.boxSize , direction, out RaycastHit hitInfo, Quaternion.identity, _unit.Range))
            {
                // Draw the hit box
                Gizmos.DrawWireCube(hitInfo.point, _unit.boxSize);
            }

            // Draw the initial box
            Gizmos.DrawWireCube(origin, _unit.boxSize);

            // Draw the movement path
            Gizmos.DrawLine(origin, origin + direction * _unit.Range);

        }

        
        
    }
}
