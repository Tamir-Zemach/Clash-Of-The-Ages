using Assets.Scripts.BackEnd.Enems;
using BackEnd.Base_Classes;
using Ui.Buttons.Upgrade_Buttons;
using UnityEditor;

namespace BackEnd.Project_inspector_Addons.Editor
{
    [CustomEditor(typeof(UnitUpgradeButton))]
    public class UpgradeButtonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var unitTypeProp = serializedObject.FindProperty(UnitUpgradeButton.FieldNames.UnitType);
            var statTypeProp = serializedObject.FindProperty(UnitUpgradeButton.FieldNames.StatType);
            var statBonusProp = serializedObject.FindProperty(UnitUpgradeButton.FieldNames.StatBonus);
            var statCostProp = serializedObject.FindProperty(ButtonWithCost.ButtonWithCostFields.Cost);
            var statCostIncProp = serializedObject.FindProperty(UnitUpgradeButton.FieldNames.StatCostInc);
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
            EditorGUILayout.PropertyField(statCostProp);
            EditorGUILayout.PropertyField(statCostIncProp);



            serializedObject.ApplyModifiedProperties();
        }
    }
}