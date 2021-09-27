using System;
using System.Collections.Generic;
using System.Reflection;
using PeacefulHills.Bootstrap.Tree;
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
            BootInfo boot = ((BootSelectionProxy)target).Boot;

            _headerStyle ??= "IN BigTitle";
            
            GUILayout.BeginVertical(_headerStyle);
            EditorGUILayout.LabelField(new GUIContent(boot.Type.Name, Icons.Boot16X), EditorStyles.largeLabel);
            GUILayout.Space(2f);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.BeginHorizontal();
                int i = 0;
                foreach (Type controlType in boot.FindControlTypes())
                {
                    if (i++ % 2 == 0)
                    {
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                    }
                    GUILayout.TextField(controlType.Name, GUILayout.MinWidth(40f), GUILayout.ExpandWidth(true));
                }
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        public override void OnInspectorGUI()
        {
            BootInfo boot = ((BootSelectionProxy)target).Boot;

            if (boot.TryGetGenericControl<IBootConfigurable<object>>(out IBootControl ctrl))
            {
                Type ctrlType = ctrl.GetType();
                PropertyInfo property = ctrlType.GetProperty("Config");
                object fieldValue = _configs.ContainsKey(ctrlType) ? _configs[ctrlType] : property!.GetValue(ctrl);
                
                GUI.enabled = true;
                EditorGUIType.DrawFields(property!.PropertyType, ref fieldValue);
                GUI.enabled = false;
                
                property.SetValue(ctrl, fieldValue);
                _configs[ctrlType] = fieldValue;
            }
        }
    }
}