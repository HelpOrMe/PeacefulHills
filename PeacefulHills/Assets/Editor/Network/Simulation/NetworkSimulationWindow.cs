using System;
using PeacefulHills.Network.Profiling;
using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Editor.Network.Simulation
{
    public class NetworkSimulationWindow : EditorWindow
    {
        private WorldsInitializationSettings _settings;
        
        [MenuItem("Window/Network/Simulation", priority=3000)]
        private static void MenuShow()
        {
            var window = GetWindow<NetworkSimulationWindow>();
            var icon = Resources.Load<Texture2D>("Network/Simulation/icons/window-icon");
            window.titleContent = new GUIContent("Network Simulation Tool", icon);
        }

        private void OnEnable()
        {
            AssetDatabase.Refresh();
        }

        private void OnGUI()
        {
            _settings ??= WorldsInitializationSettings.Load();
            
            EditorGUI.indentLevel++;

            _settings.SeparateWorlds = EditorGUILayout.Toggle("Separate worlds", _settings.SeparateWorlds);
            if (_settings.SeparateWorlds)
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