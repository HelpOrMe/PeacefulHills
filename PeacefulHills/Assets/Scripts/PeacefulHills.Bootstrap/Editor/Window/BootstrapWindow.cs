using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Bootstrap.Editor
{
    /// <summary>
    /// A window with a bootstrap hierarchy
    /// </summary>
    public class BootstrapWindow : EditorWindow
    {
        private Dictionary<uint, bool> _foldouts;

        private void OnEnable()
        {
            _foldouts = BootWindowCache.instance.Foldouts;
        }

        [MenuItem("Window/Bootstrap", priority=3001)]
        private static void MenuShow()
        {
            var window = GetWindow<BootstrapWindow>();
            window.titleContent = new GUIContent("Bootstrap", Icons.Boot2X);
        }

        private void OnGUI()
        {
            DrawRecursively(Root.Boot, 0);
            Repaint();
        }

        private void DrawRecursively(Boot boot, uint id)
        {
            DrawEntry(boot, id);

            if (_foldouts[id])
            {
                DrawChildren(boot, id);
            }
        }

        private void DrawEntry(Boot boot, uint id)
        {
            if (!_foldouts.ContainsKey(id))
            {
                _foldouts[id] = false;
            }

            Rect rect = EditorGUILayout.GetControlRect();
            
            UpdateSelection(rect, boot);

            var label = new GUIContent(GetName(boot), Icons.Boot2X);
            
            if (boot.Children.Count > 0)
            {
                _foldouts[id] = EditorGUI.Foldout(rect, _foldouts[id], label);
            }
            else
            {
                EditorGUI.LabelField(rect, label);
            }
        }
        
        private string GetName(Boot boot)
        {
            string name = boot.GetType().Name;
            return boot.Instructions.TryFind(out BootName instruction) 
                ? instruction.Name 
                : Regex.Replace(name, "(Bootstrap|Boot)$", "");
        }

        private void UpdateSelection(Rect rect, Boot boot)
        {
            if (Selection.activeObject is BootSelectionProxy proxy && proxy.Boot.Equals(boot))
            {
                Rect backgroundRect = rect;
                // Expand the rect to fully cover the area around the field
                backgroundRect.min += new Vector2(-5, -1);
                backgroundRect.max += new Vector2(5, 1);
                EditorGUI.DrawRect(backgroundRect, new Color(44 / 255f, 93 / 255f, 135 / 255f));
            }

            Rect triggerRect = rect;
            // Reduce the size of the trigger rect to add free space for
            // clicking on the foldout button
            triggerRect.min += Vector2.right * 18;
            
            if (triggerRect.Contains(Event.current.mousePosition) 
                && Event.current.type == EventType.MouseDown)
            {
                BootSelectionProxy.Select(boot);
            }
        }
        
        private void DrawChildren(Boot boot, uint id)
        {
            EditorGUI.indentLevel++;
            foreach (Boot child in boot.Children)
            {
                uint childId = (uint)boot.GetType().GetHashCode();
                uint childHierarchyId = math.hash(new uint2(id, childId));
                DrawRecursively(child, childHierarchyId);
            }
            EditorGUI.indentLevel--;
        }
    }
}