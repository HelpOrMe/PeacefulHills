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
        private readonly Dictionary<uint, bool> _entries = new Dictionary<uint, bool>();

        [MenuItem("Window/Bootstrap", priority=3001)]
        private static void MenuShow()
        {
            var window = GetWindow<BootstrapWindow>();
            window.titleContent = new GUIContent("Bootstrap");
        }

        private void OnGUI()
        {
            DrawRecursively(Boot.Root, 0);
        }

        private void DrawRecursively(IBootBranch branch, uint id)
        {
            if (!_entries.ContainsKey(id))
            {
                _entries[id] = false;
            }
            
            string name = branch.Boot.Type.Name;
            name = branch.Boot.TryGetControl(out IBootName ctrl) 
                ? ctrl.Name 
                : Regex.Replace(name, "(Bootstrap|Boot)$", "");
            
            if (branch.Children.Count > 0)
            {
                _entries[id] = EditorGUILayout.Foldout(_entries[id], name);
            }
            else
            {
                EditorGUILayout.LabelField(name);
            }

            if (_entries[id])
            {
                EditorGUI.indentLevel++;
                foreach (IBootBranch child in branch.Children)
                {
                    uint childId = (uint)name.GetHashCode();
                    uint childHierarchyId = math.hash(new uint2(id, childId));
                    DrawRecursively(child, childHierarchyId);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}