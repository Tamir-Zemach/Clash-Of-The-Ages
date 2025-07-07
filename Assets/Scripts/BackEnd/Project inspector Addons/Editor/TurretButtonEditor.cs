using UnityEditor;
using Assets.Scripts.Enems;
using Assets.Scripts.Ui.TurretButton;

[CustomEditor(typeof(TurretButton))]
public class TurretButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var turretButtonTypeProp = serializedObject.FindProperty(TurretButton.FieldNames.TurretButtonType);
        var turretTypeProp = serializedObject.FindProperty(TurretButton.FieldNames.TurretType);
        var costProp = serializedObject.FindProperty(TurretButton.FieldNames.Cost);
        var refundProp = serializedObject.FindProperty(TurretButton.FieldNames.Refund);
        var spawnPointsParent = serializedObject.FindProperty(TurretButton.FieldNames.SpawnPointParent);

        EditorGUILayout.PropertyField(turretButtonTypeProp);

        if ((TurretButtonType)turretButtonTypeProp.enumValueIndex == TurretButtonType.DeployTurret)
        {
            EditorGUILayout.PropertyField(turretTypeProp);
        }

        EditorGUILayout.PropertyField(spawnPointsParent);

        if ((TurretButtonType)turretButtonTypeProp.enumValueIndex != TurretButtonType.SellTurret)
        {
            EditorGUILayout.PropertyField(costProp);
        }

        if ((TurretButtonType)turretButtonTypeProp.enumValueIndex == TurretButtonType.SellTurret)
        {
            EditorGUILayout.PropertyField(refundProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}