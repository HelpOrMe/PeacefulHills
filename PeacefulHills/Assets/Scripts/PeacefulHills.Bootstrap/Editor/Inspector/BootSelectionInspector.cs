using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Bootstrap.Editor
{
    [CustomEditor(typeof(BootSelectionProxy))]
    public class BootSelectionInspector : UnityEditor.Editor
    {
        private Dictionary<Type, object> _configs = new Dictionary<Type, object>();
        private GUIStyle _headerStyle;

        private void OnEnable()
        {
            _configs = BootConfigs.instance.Configs;
        }

        protected override void OnHeaderGUI()
        {
            Boot boot = ((BootSelectionProxy)target).Boot;

            // Default style of the header content in the inspector
            _headerStyle ??= "IN BigTitle";
            
            GUILayout.BeginVertical(_headerStyle);
            EditorGUILayout.LabelField(new GUIContent(boot.GetType().Name, Icons.Boot16X), EditorStyles.largeLabel);
            GUILayout.Space(2f);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.BeginHorizontal();
                int i = 0;
                foreach (IBootInstruction instruction in boot.Instructions)
                {
                    // Separate items by 2 in a row
                    if (i++ % 2 == 0)
                    {
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                    }
                    GUILayout.TextField(
                        instruction.GetType().Name, GUILayout.MinWidth(40f), GUILayout.ExpandWidth(true));
                }
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        public override void OnInspectorGUI()
        {
            Boot boot = ((BootSelectionProxy)target).Boot;

            Type bootType = boot.GetType();
            if (!bootType.IsGenericTypeDefinition) return;
            
            PropertyInfo property = bootType.GetProperty("Config");
            if (property == null) return;
            
            GUILayout.Space(10f);
            object propertyValue = _configs.ContainsKey(bootType) ? _configs[bootType] : property!.GetValue(boot);
                
            GUI.enabled = true;
            EditorGUIType.DrawTypeFields(property!.PropertyType, ref propertyValue);
            GUI.enabled = false;
            
            property.SetValue(boot, propertyValue);
            _configs[bootType] = propertyValue;
        }
    }
}