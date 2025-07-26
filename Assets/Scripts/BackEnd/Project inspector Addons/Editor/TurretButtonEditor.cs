using Assets.Scripts.BackEnd.Enems;
using BackEnd.Base_Classes;
using UnityEditor;
using TurretButton = Ui.Buttons.Turret_Button.TurretButton;


namespace BackEnd.Project_inspector_Addons.Editor
{
    [CustomEditor(typeof(TurretButton))]
    public class TurretButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var turretButtonTypeProp = serializedObject.FindProperty(TurretButton.FieldNames.TurretButtonType);
            var turretTypeProp = serializedObject.FindProperty(TurretButton.FieldNames.TurretType);
            var costProp = serializedObject.FindProperty(ButtonWithCost.ButtonWithCostFields.Cost); 
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
}