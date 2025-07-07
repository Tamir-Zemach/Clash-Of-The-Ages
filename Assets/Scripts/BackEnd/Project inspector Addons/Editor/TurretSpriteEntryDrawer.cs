using UnityEditor;
using UnityEngine;
using Assets.Scripts.BackEnd.Utilities;
using Assets.Scripts.Enems;

[CustomPropertyDrawer(typeof(SpriteEntries.TurretSpriteEntry))]
public class TurretSpriteEntryDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        bool showTurretType = ShouldShowTurretType(property);
        int lines = showTurretType ? 4 : 3;
        return EditorGUIUtility.singleLineHeight * lines + 8;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = 2f;

        Rect fieldRect = new(position.x, position.y, position.width, lineHeight);

        SerializedProperty turretButtonTypeProp = property.FindPropertyRelative("TurretButtonType");
        SerializedProperty turretTypeProp = property.FindPropertyRelative("TurretType");
        SerializedProperty spriteProp = property.FindPropertyRelative("Sprite");

        // Draw TurretButtonType
        EditorGUI.PropertyField(fieldRect, turretButtonTypeProp);
        fieldRect.y += lineHeight + spacing;

        // Auto-reset TurretType if button is not Deploy
        if (!ShouldShowTurretType(turretButtonTypeProp) && turretTypeProp.enumValueIndex != (int)TurretType.None)
        {
            turretTypeProp.enumValueIndex = (int)TurretType.None;
        }

        // Conditionally draw TurretType
        if (ShouldShowTurretType(turretButtonTypeProp))
        {
            EditorGUI.PropertyField(fieldRect, turretTypeProp);
            fieldRect.y += lineHeight + spacing;
        }

        // Draw Sprite
        EditorGUI.PropertyField(fieldRect, spriteProp);
        fieldRect.y += lineHeight + spacing;

        EditorGUI.EndProperty();
    }

    private bool ShouldShowTurretType(SerializedProperty turretButtonTypeProp)
    {
        // Protect against null or non-enum property access
        if (turretButtonTypeProp == null || turretButtonTypeProp.propertyType != SerializedPropertyType.Enum)
            return false;

        return turretButtonTypeProp.enumValueIndex == (int)TurretButtonType.DeployTurret;
    }
}