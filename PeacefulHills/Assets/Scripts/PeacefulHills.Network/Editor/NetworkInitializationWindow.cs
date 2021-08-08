using System;
using PeacefulHills.Network.Profiling;
using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Network.Editor
{
    public class NetworkSimulationWindow : EditorWindow
    {
        private NetworkWorldsInitSettings _settings;
        
        [MenuItem("Window/Network/Initialization", priority=3000)]
        private static void MenuShow()
        {
            var window = GetWindow<NetworkSimulationWindow>();
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Scripts/Network/Editor Resources/icons/window-icon");
            window.titleContent = new GUIContent("Network Initialization Tool", icon);
        }

        private void OnEnable()
        {
            AssetDatabase.Refresh();
        }

        private void OnGUI()
        {
            _settings ??= NetworkWorldsInitSettings.Load();
            
            EditorGUI.indentLevel++;

            _settings.SplitWorlds = EditorGUILayout.Toggle("Split worlds", _settings.SplitWorlds);
            if (_settings.SplitWorlds)
            {
                _settings.ClientCount = Math.Max(1, EditorGUILayout.IntField("Client count", _settings.ClientCount));
            }
            
            EditorGUI.indentLevel--;
        }

        private void OnDisable()
        {
            AssetDatabase.SaveAssets();
        }
    }
}