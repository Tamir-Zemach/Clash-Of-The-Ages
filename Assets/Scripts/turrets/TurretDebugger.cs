using Assets.Scripts.turrets;
using UnityEngine;

[RequireComponent(typeof(TurretBaseBehavior))]

public class TurretDebugger : MonoBehaviour
{
    private TurretData _turretData;
    private TurretBaseBehavior TurretBaseBehavior;

    [SerializeField] private Color _boxColor = Color.red;

    private void OnDrawGizmos()
    {
        if (TurretBaseBehavior == null)
            TurretBaseBehavior = gameObject.GetComponent<TurretBaseBehavior>();
        if (_turretData == null)
            _turretData = GameStateManager.Instance.GetFriendlyTurret(TurretBaseBehavior.Type);


        Gizmos.color = _boxColor;

        // Perform the BoxCast
        if (Physics.BoxCast(TurretBaseBehavior.Origin,
            _turretData.BoxSize,
            TurretBaseBehavior.Direction, 
            out RaycastHit hitInfo,
            TurretBaseBehavior.Rotation, 
            _turretData.Range,
            _turretData.OppositeUnitLayer))
        {
            // Draw the hit box
            Gizmos.DrawWireCube(hitInfo.point, _turretData.BoxSize);
        }

        // Draw the initial box
        Gizmos.DrawWireCube(TurretBaseBehavior.Origin, _turretData.BoxSize);

        // Draw the movement path
        Gizmos.DrawLine(TurretBaseBehavior.Origin,
            TurretBaseBehavior.Origin + TurretBaseBehavior.Direction * _turretData.Range);

    }

}
