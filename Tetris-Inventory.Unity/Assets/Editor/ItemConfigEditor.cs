using Runtime.InventorySystem.Common;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ItemConfig))]
    public sealed class ItemConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var config = (ItemConfig)target;
            config.id = EditorGUILayout.TextField("ID", config.id);
            config.displayName = EditorGUILayout.TextField("Display Name", config.displayName);
            
            EditorGUILayout.LabelField("Description", EditorStyles.boldLabel);
            config.description = EditorGUILayout.TextArea(config.description, GUILayout.MinHeight(60));
            
            config.color = EditorGUILayout.ColorField("Color", config.color);
            
            EditorGUILayout.Space(10); 
            EditorGUILayout.LabelField("Shape", EditorStyles.boldLabel);
            
            config.width = Mathf.Max(1, EditorGUILayout.IntField("Width", config.width));
            config.height = Mathf.Max(1, EditorGUILayout.IntField("Height", config.height));
            
            var totalCells = config.width * config.height;
            
            while (config.flatShape.Count < totalCells) config.flatShape.Add(false);

            while (config.flatShape.Count > totalCells) config.flatShape.RemoveAt(config.flatShape.Count - 1);

            for (var y = 0; y < config.height; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (var x = 0; x < config.width; x++)
                {
                    var index = y * config.width + x;
                    config.flatShape[index] = GUILayout.Toggle(config.flatShape[index], GUIContent.none, GUILayout.Width(20));
                    
                }
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.Space(5);

            if (GUILayout.Button("Clear Shape"))
            {
                for (var i = 0; i < config.flatShape.Count; i++)
                {
                    config.flatShape[i] = false;
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(config);
            }
        }
    }
}