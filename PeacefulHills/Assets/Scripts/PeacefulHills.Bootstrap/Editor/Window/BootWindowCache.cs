using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PeacefulHills.Bootstrap.Editor
{
    [FilePath("Boot/WindowCache.asset", FilePathAttribute.Location.ProjectFolder)]
    public class BootWindowCache : ScriptableSingleton<BootWindowCache>, ISerializationCallbackReceiver
    {
        public readonly Dictionary<uint, bool> Foldouts = new Dictionary<uint, bool>();
        private List<uint> _foldouts;
        
        public void OnBeforeSerialize()
        {
            _foldouts = new List<uint>();
            
            foreach (KeyValuePair<uint, bool> pair in Foldouts)
            {
                if (pair.Value)
                {
                    _foldouts.Add(pair.Key);
                }
            }
        }

        public void OnAfterDeserialize()
        {
            foreach (uint foldout in _foldouts)
            {
                Foldouts[foldout] = true;
            }
        }
    }
}