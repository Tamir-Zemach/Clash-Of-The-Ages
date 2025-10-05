using BackEnd.Data__ScriptableOBj_;
using BackEnd.Data_Getters;
using turrets;
using UnityEngine;

namespace Debugging
{
    [RequireComponent(typeof(TurretBaseBehavior))]

    public class TurretDebugger : MonoBehaviour
    {
        private TurretData _turretData;
        private TurretBaseBehavior _turretBaseBehavior;

        private Color _boxColor = Color.green;

        private void OnDrawGizmos()
        {
            if (_turretBaseBehavior == null)
                _turretBaseBehavior = gameObject.GetComponent<TurretBaseBehavior>();
            if (_turretData == null) return;
                _turretData = GameDataRepository.Instance.FriendlyTurrets.GetData(_turretBaseBehavior.Turret.Type);

            Gizmos.color = _boxColor;

            Vector3 origin = _turretBaseBehavior.Origin;
            Vector3 direction = _turretBaseBehavior.Direction;
            Quaternion rotation = _turretBaseBehavior.Rotation;
            
            if (Physics.BoxCast(origin, _turretData.BoxSize / 2f, direction, out RaycastHit hitInfo, rotation,
                    _turretData.Range, _turretData.OppositeUnitLayer))
            {
                    Gizmos.color = Color.red;
            }
            
            
            // Save current matrix
            Matrix4x4 oldMatrix = Gizmos.matrix;

            // Set gizmo matrix to match the cast direction
            Gizmos.matrix = Matrix4x4.TRS(origin + direction * (_turretData.Range / 2f), rotation, Vector3.one);

            // Draw the box representing the full cast volume
            Vector3 castSize = _turretData.BoxSize + new Vector3(0, 0, _turretData.Range);
            Gizmos.DrawWireCube(Vector3.zero, castSize);

            // Restore matrix
            Gizmos.matrix = oldMatrix;

            // Optional: Draw origin and direction line for clarity
            Gizmos.DrawWireCube(origin, _turretData.BoxSize);
            Gizmos.DrawLine(origin, origin + direction * _turretData.Range);
        }

    }
}
