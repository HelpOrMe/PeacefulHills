using System.Collections.Generic;
using System.Text.RegularExpressions;
using PeacefulHills.Bootstrap.Tree;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Bootstrap.Editor
{
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
            DrawRecursively(Boot.Root, 0);
            Repaint();
        }

        private void DrawRecursively(IBootBranch branch, uint id)
        {
            DrawEntry(branch, id);

            if (_foldouts[id])
            {
                DrawChildren(branch, id);
            }
        }

        private void DrawEntry(IBootBranch branch, uint id)
        {
            if (!_foldouts.ContainsKey(id))
            {
                _foldouts[id] = false;
            }

            Rect rect = EditorGUILayout.GetControlRect();
            UpdateSelection(rect, branch.Boot);

            var label = new GUIContent(GetName(branch.Boot), Icons.Boot2X);
            
            if (branch.Children.Count > 0)
            {
                _foldouts[id] = EditorGUI.Foldout(rect, _foldouts[id], label);
            }
            else
            {
                EditorGUI.LabelField(rect, label);
            }
        }
        
        private string GetName(BootInfo boot)
        {
            string name = boot.Type.Name;
            return boot.TryGetControl(out IBootNamed named) 
                ? named.Name 
                : Regex.Replace(name, "(Bootstrap|Boot)$", "");
        }

        private void UpdateSelection(Rect rect, BootInfo boot)
        {
            if (Selection.activeObject is BootSelectionProxy proxy && proxy.Boot.Equals(boot))
            {
                Rect backgroundRect = rect;
                backgroundRect.min += new Vector2(-5, -1);
                backgroundRect.max += new Vector2(5, 1);
                EditorGUI.DrawRect(backgroundRect, new Color(44 / 255f, 93 / 255f, 135 / 255f));
            }

            Rect triggerRect = rect;
            triggerRect.min += Vector2.right * 18;
            
            if (triggerRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
            {
                BootSelectionProxy.Select(boot);
            }
        }
        
        private void DrawChildren(IBootBranch branch, uint id)
        {
            EditorGUI.indentLevel++;
            foreach (IBootBranch child in branch.Children)
            {
                uint childId = (uint)branch.Boot.Type.GetHashCode();
                uint childHierarchyId = math.hash(new uint2(id, childId));
                DrawRecursively(child, childHierarchyId);
            }
            EditorGUI.indentLevel--;
        }
    }
}