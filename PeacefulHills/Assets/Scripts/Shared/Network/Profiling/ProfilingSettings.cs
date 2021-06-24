#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Network.Profiling
{
    public class ProfilingSettings : ScriptableObject, IProfilingSettings
    {
        [field: SerializeField] public bool SeparateWorlds { get; set; }
        [field: SerializeField] public int ClientCount { get; set; } = 1;

        public static ProfilingSettings Load()
        {
            const string path = "Assets/Editor/Resources/Network/Simulation/Settings.asset";
            
            var settings = AssetDatabase.LoadAssetAtPath<ProfilingSettings>(path);
            if (settings == null)
            {
                settings = CreateInstance<ProfilingSettings>();
                AssetDatabase.CreateAsset(settings, path);
            }
            return settings;
        }
    }
}

#endif