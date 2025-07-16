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
        var overLay = serializedObject.FindProperty(TurretButton.FieldNames.OverLay);

        EditorGUILayout.PropertyField(turretButtonTypeProp);

        if ((TurretButtonType)turretButtonTypeProp.enumValueIndex == TurretButtonType.DeployTurret)
        {
            EditorGUILayout.PropertyField(turretTypeProp);
        }

        if ((TurretButtonType)turretButtonTypeProp.enumValueIndex != TurretButtonType.SellTurret)
        {
            EditorGUILayout.PropertyField(costProp);
        }

        if ((TurretButtonType)turretButtonTypeProp.enumValueIndex == TurretButtonType.SellTurret)
        {
            EditorGUILayout.PropertyField(refundProp);
        }

        EditorGUILayout.PropertyField(overLay);

        serializedObject.ApplyModifiedProperties();
    }
}