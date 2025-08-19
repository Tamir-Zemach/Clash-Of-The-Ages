using UnityEditor;
using UnityEngine;

namespace BackEnd.Project_inspector_Addons.Editor
{
    public class ComponentHelperTool : EditorWindow
    {
        GameObject source;
        GameObject target;
        GameObject gameObjectToClearComponents;

        [MenuItem("Tools/Component Copier")]
        public static void ShowWindow()
        {
            GetWindow<ComponentHelperTool>("Component Copier");
        }

        void OnGUI()
        {
            GUILayout.Label("Component Copier Tool", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox("Select the source GameObject to copy components from.", MessageType.Info);
            source = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Source", "GameObject to copy components from"), source, typeof(GameObject), true);

            EditorGUILayout.HelpBox("Select the target GameObject to copy components to. Existing components will be replaced.", MessageType.Info);
            target = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Target", "GameObject to copy components to"), target, typeof(GameObject), true);

            GUILayout.Space(10);

            if (GUILayout.Button("Copy Components (Including Transform)"))
            {
                CopyComponents();
            }

            GUILayout.Space(20);
            GUILayout.Label("Clear Components Tool", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox("Select the GameObject whose components (except Transform) will be removed.", MessageType.Info);
            gameObjectToClearComponents = (GameObject)EditorGUILayout.ObjectField(new GUIContent("GameObject to Clear", "GameObject to remove all components from (except Transform)"), gameObjectToClearComponents, typeof(GameObject), true);

            if (GUILayout.Button("Clear All Components (Except Transform)"))
            {
                ClearComponents();
            }
        }

        void CopyComponents()
        {
            if (source == null || target == null)
            {
                Debug.LogWarning("ComponentHelperTool: Source or Target is null.");
                return;
            }

            foreach (var comp in source.GetComponents<Component>())
            {
                // Remove existing component of the same type
                var existing = target.GetComponent(comp.GetType());
                if (existing != null)
                {
                    DestroyImmediate(existing);
                }

                UnityEditorInternal.ComponentUtility.CopyComponent(comp);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(target);
            }

            Debug.Log($"Copied all components (including Transform) from '{source.name}' to '{target.name}'.");
        }

        void ClearComponents()
        {
            if (gameObjectToClearComponents == null)
            {
                Debug.LogWarning("ComponentHelperTool: GameObject to clear is null.");
                return;
            }

            var components = gameObjectToClearComponents.GetComponents<Component>();
            foreach (var comp in components)
            {
                if (comp is Transform) continue;

                DestroyImmediate(comp);
            }

            Debug.Log($"Cleared all components from '{gameObjectToClearComponents.name}' except Transform.");
        }
    }
}