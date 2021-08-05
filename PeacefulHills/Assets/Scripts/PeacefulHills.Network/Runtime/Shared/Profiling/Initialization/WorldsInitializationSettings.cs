#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Network.Profiling
{
    public class WorldsInitializationSettings : ScriptableObject
    {
        public bool hostWorld = true;
        public int clientCount = 1;

        public static WorldsInitializationSettings Load()
        {
            const string path = "Assets/Scripts/PeacefulHills.Network/Editor Resources/InitializationSettings.asset";

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