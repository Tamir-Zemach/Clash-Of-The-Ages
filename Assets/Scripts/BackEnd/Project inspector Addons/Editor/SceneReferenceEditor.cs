#if UNITY_EDITOR
using BackEnd.Project_inspector_Addons;
using UnityEditor;
using UnityEngine;
//TODO: Fix SHITSHOW 2.1 - ugly and unnecessary code 
[CustomEditor(typeof(MonoBehaviour), true)]
public class SceneReferenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var targetObject = target as MonoBehaviour;
        var fields = targetObject.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

        foreach (var field in fields)
        {
            if (field.FieldType == typeof(SceneReference))
            {
                SceneReference sceneRef = field.GetValue(targetObject) as SceneReference;
                if (sceneRef != null)
                {
                    if (GUILayout.Button($"Update Scene Path for {field.Name}"))
                    {
                        sceneRef.UpdateScenePath(); // ✅ Your method goes here
                        EditorUtility.SetDirty(targetObject);
                    }
                }
            }
        }
    }
}
#endif