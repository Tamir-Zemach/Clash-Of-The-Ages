using Assets.Scripts.BackEnd.Enems;
using UnityEditor;
using UnityEngine;
using static SpritesLevelUpData;

[CustomPropertyDrawer(typeof(TurretKey))]
public class TurretKeyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var buttonTypeProp = property.FindPropertyRelative(nameof(TurretKey.ButtonType));
        bool showTurretType = ShouldShowTurretType(buttonTypeProp);
        int lines = showTurretType ? 2 : 1;

        return EditorGUIUtility.singleLineHeight * lines + 4;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var buttonTypeProp = property.FindPropertyRelative(nameof(TurretKey.ButtonType));
        var turretTypeProp = property.FindPropertyRelative(nameof(TurretKey.TurretType));

        EditorGUI.BeginProperty(position, label, property);
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = 2f;

        Rect fieldRect = new(position.x, position.y, position.width, lineHeight);

        // Draw ButtonType
        EditorGUI.PropertyField(fieldRect, buttonTypeProp, new GUIContent("Button Type"));
        fieldRect.y += lineHeight + spacing;

        // Conditionally draw TurretType
        if (ShouldShowTurretType(buttonTypeProp))
        {
            EditorGUI.PropertyField(fieldRect, turretTypeProp, new GUIContent("Turret Type"));
        }

        EditorGUI.EndProperty();
    }

    private bool ShouldShowTurretType(SerializedProperty buttonTypeProp)
    {
        return buttonTypeProp.enumValueIndex == (int)TurretButtonType.DeployTurret;
    }
}