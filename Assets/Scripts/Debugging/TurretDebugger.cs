using BackEnd.Data__ScriptableOBj_;
using UnityEngine;

namespace turrets
{
    [RequireComponent(typeof(TurretBaseBehavior))]

    public class TurretDebugger : MonoBehaviour
    {
        private TurretData _turretData;
        private TurretBaseBehavior _turretBaseBehavior;

        [SerializeField] private Color _boxColor = Color.red;

        private void OnDrawGizmos()
        {
            if (_turretBaseBehavior == null)
                _turretBaseBehavior = gameObject.GetComponent<TurretBaseBehavior>();
            if (_turretData == null)
                _turretData = GameDataRepository.Instance.FriendlyTurrets.GetData(_turretBaseBehavior.Turret.Type);


            Gizmos.color = _boxColor;

            // Perform the BoxCast
            if (Physics.BoxCast(_turretBaseBehavior.Origin,
                    _turretData.BoxSize,
                    _turretBaseBehavior.Direction, 
                    out var hitInfo,
                    _turretBaseBehavior.Rotation, 
                    _turretData.Range,
                    _turretData.OppositeUnitLayer))
            {
                // Draw the hit box
                Gizmos.DrawWireCube(hitInfo.point, _turretData.BoxSize);
            }

            // Draw the initial box
            Gizmos.DrawWireCube(_turretBaseBehavior.Origin, _turretData.BoxSize);

            // Draw the movement path
            Gizmos.DrawLine(_turretBaseBehavior.Origin,
                _turretBaseBehavior.Origin + _turretBaseBehavior.Direction * _turretData.Range);

        }

    }
}
