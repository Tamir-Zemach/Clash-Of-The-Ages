using Assets.Scripts.Enems;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManagerDebugger : EditorWindow
{
    private Vector2 _scroll;

    private bool _showUnitCosts = true;
    private bool _showPlayerCosts = true;
    private bool _showSpecialAttackPrefabs = true;
    private bool _showTurretPrefabs = true;
    private bool _showFriendlyUnitPrefabs = true;
    private bool _showEnemyUnitPrefabs = true;

    [MenuItem("Tools/Game State Manager Debugger")]
    public static void ShowWindow()
    {
        GetWindow<GameStateManagerDebugger>("Game State Manager Debugger");
    }

    private void OnGUI()
    {
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Enter Play Mode to inspect GameStateManager data.", MessageType.Info);
            return;
        }

        var manager = GameStateManager.Instance;
        if (manager == null)
        {
            EditorGUILayout.HelpBox("GameStateManager not found.", MessageType.Warning);
            return;
        }

        EditorGUILayout.LabelField("Active Scene:", SceneManager.GetActiveScene().name);
        EditorGUILayout.Space();

        _scroll = EditorGUILayout.BeginScrollView(_scroll);

        EditorGUILayout.BeginVertical("box");
        DrawSection(ref _showUnitCosts, "Unit Stat Upgrade Costs", () =>
        {
            var sorted = manager.GetAllUnitStatUpgradeCosts().GetAll()
                .OrderBy(kvp => kvp.Key.Item1)
                .ThenBy(kvp => kvp.Key.Item2);

            foreach (var kvp in sorted)
                EditorGUILayout.LabelField($"{kvp.Key.Item1} / {kvp.Key.Item2}", kvp.Value.ToString());
        });
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        DrawSection(ref _showPlayerCosts, "Player Stat Upgrade Costs", () =>
        {
            var sorted = manager.GetAllPlayerUpgradeCosts().GetAll()
                .OrderBy(kvp => kvp.Key);

            foreach (var kvp in sorted)
                EditorGUILayout.LabelField($"{kvp.Key}", kvp.Value.ToString());
        });
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        DrawSection(ref _showFriendlyUnitPrefabs, "Friendly Unit Prefabs", () =>
        {
            foreach (var unit in manager.GetAllFriendlyUnits().OrderBy(u => u.Type))
            {
                if (unit.Prefab != null)
                {
                    EditorGUILayout.ObjectField($"{unit.Type}", unit.Prefab, typeof(GameObject), false);
                }
                else
                {
                    EditorGUILayout.LabelField($"{unit.Type}", "No prefab assigned");
                }
            }
        });
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        DrawSection(ref _showEnemyUnitPrefabs, "Enemy Unit Prefabs", () =>
        {
            foreach (var unit in manager.GetAllEnemyUnits().OrderBy(u => u.Type))
            {
                if (unit.Prefab != null)
                {
                    EditorGUILayout.ObjectField($"{unit.Type}", unit.Prefab, typeof(GameObject), false);
                }
                else
                {
                    EditorGUILayout.LabelField($"{unit.Type}", "No prefab assigned");
                }
            }
        });
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        DrawSection(ref _showSpecialAttackPrefabs, "Special Attack Prefabs", () =>
        {
            foreach (var kvp in manager.GetAllSpecialAttackPrefabs().GetAll()
                     .OrderBy(kvp => kvp.Key))
            {
                EditorGUILayout.ObjectField($"{kvp.Key}", kvp.Value, typeof(GameObject), false);
            }
        });
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        DrawSection(ref _showTurretPrefabs, "Turret Prefabs", () =>
        {
            foreach (var kvp in manager.GetAllTurretPrefabs().GetAll()
                     .OrderBy(kvp => kvp.Key))
            {
                EditorGUILayout.ObjectField($"{kvp.Key}", kvp.Value, typeof(GameObject), false);
            }
        });
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    private void DrawSection(ref bool toggle, string title, System.Action drawContent)
    {
        toggle = EditorGUILayout.Foldout(toggle, title, true, EditorStyles.foldoutHeader);
        if (toggle)
        {
            EditorGUI.indentLevel++;
            drawContent?.Invoke();
            EditorGUI.indentLevel--;
        }
    }
}