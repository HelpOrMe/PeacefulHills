#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Network.Profiling
{
    public class NetworkWorldsInitSettings : ScriptableObject, INetworkWorldsInitSettings
    {
        public static INetworkWorldsInitSettings Current
        {
            get => _current ??= Load();
            set => _current = value;
        }

        private static INetworkWorldsInitSettings _current;
        
        [field: SerializeField] public bool SplitWorlds { get; set; } = true;
        [field: SerializeField] public int ClientCount { get; set; } = 1;

        public static NetworkWorldsInitSettings Load()
        {
            const string path = "Assets/Scripts/PeacefulHills.Network/Editor Resources/InitializationSettings.asset";

            var settings = AssetDatabase.LoadAssetAtPath<NetworkWorldsInitSettings>(path);
            if (settings == null)
            {
                settings = CreateInstance<NetworkWorldsInitSettings>();
                AssetDatabase.CreateAsset(settings, path);
            }

            return settings;
        }
    }
}

#endif