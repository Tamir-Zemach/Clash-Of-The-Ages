using BackEnd.Enums;
using Ui.Buttons;
using Ui.Buttons.Upgrade_Buttons;
using Ui.Buttons.Upgrade_Popup;
using UnityEditor;


namespace BackEnd.Project_inspector_Addons.Editor
{
    [CustomEditor(typeof(UnitUpgradePopupSlot))]
    public class UnitUpgradePopupSlotEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var unitTypeProp = serializedObject.FindProperty(UnitUpgradePopupSlot.FieldNames.UnitType);
            var statTypeProp = serializedObject.FindProperty(UnitUpgradePopupSlot.FieldNames.StatType);
            var statBonusProp = serializedObject.FindProperty(UnitUpgradePopupSlot.FieldNames.StatBonus);
            var attackDelayReductionProp = serializedObject.FindProperty(UnitUpgradeButton.FieldNames.AttackDelayReductionPercent);

            EditorGUILayout.PropertyField(unitTypeProp);
            EditorGUILayout.PropertyField(statTypeProp);
            // Show the attack speed field only if statType == AttackSpeed
            if ((StatType)statTypeProp.enumValueIndex == StatType.AttackSpeed)
            {
                EditorGUILayout.PropertyField(attackDelayReductionProp);
            }
            else
            {
                EditorGUILayout.PropertyField(statBonusProp);
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}