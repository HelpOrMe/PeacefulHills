#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Network.Profiling
{
    public class WorldsInitializationSettings : ScriptableObject, IWorldsInitializationSettings
    {
        [field: SerializeField] public bool SeparateWorlds { get; set; }
        [field: SerializeField] public int ClientCount { get; set; } = 1;

        public static WorldsInitializationSettings Load()
        {
            const string path = "Assets/Editor/Resources/Network/Simulation/InitializationSettings.asset";
            
            var settings = AssetDatabase.LoadAssetAtPath<WorldsInitializationSettings>(path);
            if (settings == null)
            {
                settings = CreateInstance<WorldsInitializationSettings>();
                AssetDatabase.CreateAsset(settings, path);
            }
            return settings;
        }
    }
}

#endif
