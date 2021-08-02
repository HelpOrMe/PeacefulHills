using System;
using PeacefulHills.Network.Profiling;
using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Editor.Network.Simulation
{
    public class NetworkSimulationWindow : EditorWindow
    {
        private WorldsInitializationSettings _settings;
        
        [MenuItem("Window/Network/Initialization", priority=3000)]
        private static void MenuShow()
        {
            var window = GetWindow<NetworkSimulationWindow>();
            var icon = Resources.Load<Texture2D>("Network/icons/window-icon");
            window.titleContent = new GUIContent("Network Initialization Tool", icon);
        }

        private void OnEnable()
        {
            AssetDatabase.Refresh();
        }

        private void OnGUI()
        {
            _settings ??= WorldsInitializationSettings.Load();
            
            EditorGUI.indentLevel++;

            _settings.hostWorld = EditorGUILayout.Toggle("Host world", _settings.hostWorld);
            if (!_settings.hostWorld)
            {
                _settings.clientCount = Math.Max(1, EditorGUILayout.IntField("Client count", _settings.clientCount));
            }
            
            EditorGUI.indentLevel--;
        }

        private void OnDisable()
        {
            AssetDatabase.SaveAssets();
        }
    }
}