using Assets.Scripts.Backend.Data;
using Assets.Scripts.BackEnd.Utilities;
using Assets.Scripts.Enems;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpritesLevelUpData))]
public class SpriteComparerEditor : Editor
{
    private SpritesData referenceData;
    private bool showDiffs = false;

    private enum ComparisonState
    {
        Match,
        Mismatch,
        MissingInReference,
        MissingInTarget
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("🎨 Sprite Comparison", EditorStyles.boldLabel);

        referenceData = (SpritesData)EditorGUILayout.ObjectField(
            "Reference Sprite Data",
            referenceData,
            typeof(SpritesData),
            false
        );

        if (referenceData == null)
        {
            EditorGUILayout.HelpBox("Assign a reference SpritesData asset to compare.", MessageType.Info);
            return;
        }

        if (GUILayout.Button(showDiffs ? "🙈 Hide Sprite Differences" : "🔍 Show Sprite Differences"))
        {
            showDiffs = !showDiffs;
        }

        if (showDiffs)
        {
            ShowAllComparisons((SpritesLevelUpData)target);
        }

        EditorGUILayout.Space(5);
    }

    private void ShowAllComparisons(SpritesLevelUpData levelUpData)
    {
        CompareCategory("🔧 Upgrade Buttons",
            SpritesLevelUpData.BuildDictionary<(UnitType, StatType), SpriteEntries.UpgradeButtonSpriteEntry>(referenceData.unitUpgradeButtonSpriteMap),
            SpritesLevelUpData.BuildDictionary<(UnitType, StatType), SpriteEntries.UpgradeButtonSpriteEntry>(levelUpData.unitUpgradeButtonSpriteMap));

        CompareCategory("👤 Unit Sprites",
            SpritesLevelUpData.BuildDictionary<UnitType, SpriteEntries.UnitSpriteEntry>(referenceData.unitSpriteMap),
            SpritesLevelUpData.BuildDictionary<UnitType, SpriteEntries.UnitSpriteEntry>(levelUpData.unitSpriteMap));

        CompareCategory("💥 Special Attacks",
            SpritesLevelUpData.BuildDictionary<AgeStageType, SpriteEntries.SpecialAttackSpriteEntry>(referenceData.specialAttackSpriteMap),
            SpritesLevelUpData.BuildDictionary<AgeStageType, SpriteEntries.SpecialAttackSpriteEntry>(levelUpData.specialAttackSpriteMap));

        CompareCategory("🛡️ Turret Sprites",
            SpritesLevelUpData.BuildDictionary<(TurretType, TurretButtonType), SpriteEntries.TurretSpriteEntry>(referenceData.turretSpriteMap),
            SpritesLevelUpData.BuildDictionary<(TurretType, TurretButtonType), SpriteEntries.TurretSpriteEntry>(levelUpData.turretSpriteMap));
    }

    private void CompareCategory<TKey>(string sectionLabel, Dictionary<TKey, Sprite> referenceDict, Dictionary<TKey, Sprite> targetDict)
        where TKey : notnull
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(sectionLabel, EditorStyles.boldLabel);

        HashSet<TKey> allKeys = new(referenceDict.Keys);
        allKeys.UnionWith(targetDict.Keys);

        foreach (var key in allKeys)
        {
            referenceDict.TryGetValue(key, out var baseSprite);
            targetDict.TryGetValue(key, out var targetSprite);

            var state = GetComparisonState(baseSprite, targetSprite);
            DrawGenericComparisonEntry(key.ToString(), baseSprite, targetSprite, state);
        }
    }

    private ComparisonState GetComparisonState(Sprite baseSprite, Sprite targetSprite)
    {
        if (baseSprite == null && targetSprite != null) return ComparisonState.MissingInReference;
        if (baseSprite != null && targetSprite == null) return ComparisonState.MissingInTarget;
        if (baseSprite == targetSprite) return ComparisonState.Match;
        return ComparisonState.Mismatch;
    }

    private void DrawGenericComparisonEntry(string keyLabel, Sprite baseSprite, Sprite targetSprite, ComparisonState state)
    {
        Color labelColor = state switch
        {
            ComparisonState.Match => Color.gray,
            ComparisonState.Mismatch => Color.yellow,
            ComparisonState.MissingInReference => Color.red,
            ComparisonState.MissingInTarget => Color.cyan,
            _ => Color.white
        };

        var style = new GUIStyle(EditorStyles.boldLabel) { normal = { textColor = labelColor } };

        EditorGUILayout.Space(4);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField($"{keyLabel} — {state}", style);
        EditorGUILayout.BeginHorizontal();
        DrawSprite("💾 Base", baseSprite);
        DrawSprite("🎯 LevelUp", targetSprite);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void DrawSprite(string label, Sprite sprite)
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(80));
        EditorGUILayout.LabelField(label, GUILayout.Width(75));
        Texture2D tex = sprite != null ? AssetPreview.GetAssetPreview(sprite) : Texture2D.grayTexture;
        GUILayout.Label(tex, GUILayout.Width(64), GUILayout.Height(64));
        EditorGUILayout.EndVertical();
    }
}