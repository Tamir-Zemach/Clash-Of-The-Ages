using BackEnd.Data__ScriptableOBj_;
using units.Behavior;
using UnityEngine;

namespace Debugging
{
    public class UnitDebugger : MonoBehaviour
    {
        private UnitBaseBehaviour _unitBaseBehaviour;
        private UnitData  _unit;

        private void Start()
        {
            _unitBaseBehaviour = GetComponent<UnitBaseBehaviour>();
            _unit = _unitBaseBehaviour.Unit;
        }


        private void OnDrawGizmos()
        {
            if (_unit == null) return;

            Vector3 origin = transform.position;
            Vector3 direction = transform.forward;

            Gizmos.color = Color.green;

            // Check for detection
            if (Physics.BoxCast(origin, _unit.boxSize / 2f, direction, out RaycastHit hitInfo, Quaternion.identity,
                    _unit.Range, _unit.FriendlyUnitMask | _unit.OppositeUnitMask))
            {
                //print($"{hitInfo.transform.gameObject.name} detected");
                if (hitInfo.transform.CompareTag(_unit.FriendlyUnitTag))
                    Gizmos.color = Color.cyan;
                else if (hitInfo.transform.CompareTag(_unit.OppositeUnitTag) || hitInfo.transform.CompareTag(_unit.OppositeBaseTag))
                    Gizmos.color = Color.red;
            }

            // Save current matrix
            Matrix4x4 oldMatrix = Gizmos.matrix;

            // Set gizmo matrix to match the cast direction
            Gizmos.matrix = Matrix4x4.TRS(origin + direction * (_unit.Range / 2f), Quaternion.LookRotation(direction), Vector3.one);

            // Draw the box representing the cast volume
            Vector3 castSize = _unit.boxSize + new Vector3(0, 0, _unit.Range);
            Gizmos.DrawWireCube(Vector3.zero, castSize);

            // Restore matrix
            Gizmos.matrix = oldMatrix;
        }

        
        
    }
}
