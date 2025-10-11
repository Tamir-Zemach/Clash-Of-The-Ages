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

            Vector3 direction = transform.forward;
            Vector3 origin = transform.position - direction * (_unit.boxSize.z * 0.5f);

            Gizmos.color = Color.green;

            if (Physics.BoxCast(origin, _unit.boxSize, direction, out RaycastHit hitInfo, Quaternion.identity,
                    _unit.Range, _unit.FriendlyUnitMask | _unit.OppositeUnitMask))
            {
                if (hitInfo.transform.CompareTag(_unit.FriendlyUnitTag))
                    Gizmos.color = Color.cyan;
                else if (hitInfo.transform.CompareTag(_unit.OppositeUnitTag) || hitInfo.transform.CompareTag(_unit.OppositeBaseTag))
                    Gizmos.color = Color.red;
            }

            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(origin + direction * (_unit.Range / 2f), Quaternion.LookRotation(direction), Vector3.one);

            Vector3 castSize = _unit.boxSize + new Vector3(0, 0, _unit.Range);
            Gizmos.DrawWireCube(Vector3.zero, castSize);

            Gizmos.matrix = oldMatrix;
        }
        
        
    }
}
