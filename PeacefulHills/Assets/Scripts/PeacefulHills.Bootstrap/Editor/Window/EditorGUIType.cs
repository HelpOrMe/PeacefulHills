using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Bootstrap.Editor
{
    public static class EditorGUIType
    {
        public static void DrawFields(Type type, ref object value)
        {
            value ??= Activator.CreateInstance(type);

            foreach (FieldInfo field in type.GetFields())
            {
                object fieldValue = field.GetValue(value);
                
                if (!TryDrawDefaults(field.Name, field.FieldType, ref fieldValue))
                {
                    EditorGUILayout.LabelField(field.Name, EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    DrawFields(field.FieldType, ref fieldValue);
                    EditorGUI.indentLevel--;
                }
                
                field.SetValue(value, fieldValue);
            }
        }

        public static bool TryDrawDefaults(
            string label, Type type, ref object value,
            GUIStyle style = null, params GUILayoutOption[] options)
        {
            return TryDrawDefaults(new GUIContent(label), type, ref value, style, options);
        }
        
        public static bool TryDrawDefaults(
            GUIContent label, Type type, ref object value, 
            GUIStyle style = null, params GUILayoutOption[] options)
        {
            if (type == typeof(int))
            {
                value = EditorGUILayout.IntField(label, (int)value, style ?? EditorStyles.numberField, options);
            }
            else if (type == typeof(float))
            {
                value = EditorGUILayout.FloatField(label, (float)value, style ?? EditorStyles.numberField, options);
            }
            else if (type == typeof(double))
            {
                value = EditorGUILayout.DoubleField(label, (double)value, style ?? EditorStyles.numberField, options);
            }
            else if (type == typeof(Vector2))
            {
                value = EditorGUILayout.Vector2Field(label, (Vector2)value, options);
            }
            else if (type == typeof(Vector3))
            {
                value = EditorGUILayout.Vector3Field(label, (Vector3)value, options);
            }
            else if (type == typeof(Vector4))
            {
                value = EditorGUILayout.Vector4Field(label, (Vector4)value, options);
            }
            else if (type == typeof(Vector2Int))
            {
                value = EditorGUILayout.Vector2IntField(label, (Vector2Int)value, options);
            }
            else if (type == typeof(Vector3Int))
            {
                value = EditorGUILayout.Vector3IntField(label, (Vector3Int)value, options);
            }
            else if (type == typeof(bool))
            {
                value = EditorGUILayout.Toggle(label, (bool)value, style ?? EditorStyles.toggle, options);
            }
            else if (type == typeof(string))
            {
                value = EditorGUILayout.TextField(label, (string)value, style ?? EditorStyles.textField, options);
            }
            else if (type == typeof(Enum))
            {
                value = EditorGUILayout.EnumPopup(label, (Enum)value, style ?? EditorStyles.popup, options);
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}