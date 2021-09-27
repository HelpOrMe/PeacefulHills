using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Bootstrap.Editor
{
    [FilePath("Boot/Configs.asset", FilePathAttribute.Location.ProjectFolder)]
    public class BootConfigs : ScriptableSingleton<BootConfigs>, ISerializationCallbackReceiver
    {
        public readonly Dictionary<Type, object> Configs = new Dictionary<Type, object>();

        private string[] _bootTypeNames;
        private string[] _bootConfigTypes;
        private string[] _bootConfigJsons;
        
        public void OnBeforeSerialize()
        {
            _bootTypeNames = new string[Configs.Count];
            _bootConfigTypes = new string[Configs.Count];
            _bootConfigJsons = new string[Configs.Count];

            int i = 0;
            foreach (KeyValuePair<Type, object> pair in Configs)
            {
                _bootTypeNames[i] = pair.Key.AssemblyQualifiedName;
                _bootConfigTypes[i] = pair.Value.GetType().AssemblyQualifiedName;
                _bootConfigJsons[i] = JsonUtility.ToJson(pair.Value);
                i++;
            }
        }

        public void OnAfterDeserialize()
        {
            for (int i = 0; i < _bootTypeNames.Length; i++)
            {
                var bootType = Type.GetType(_bootTypeNames[i]);
                var configType = Type.GetType(_bootConfigTypes[i])!;
                Configs[bootType!] = JsonUtility.FromJson(_bootConfigJsons[i], configType);
            }
        }
    }
}