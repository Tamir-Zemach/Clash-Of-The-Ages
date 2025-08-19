using BackEnd.Data__ScriptableOBj_;
using units.Behavior;
using UnityEngine;

namespace Debugging
{
    [RequireComponent(typeof(UnitBaseBehaviour))]
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

            // Default color
            Gizmos.color = _unit.boxColor;

            // Check for detection
            if (Physics.BoxCast(origin, _unit.boxSize, direction, out RaycastHit hitInfo, Quaternion.identity, _unit.Range))
            {
                Gizmos.color = Color.red;
            }

            // Now draw the gizmo with the correct color
            Vector3 center = origin + direction * (_unit.Range / 2);
            Gizmos.DrawWireCube(center, _unit.boxSize + new Vector3(0, 0, _unit.Range));
        }

        
        
    }
}
