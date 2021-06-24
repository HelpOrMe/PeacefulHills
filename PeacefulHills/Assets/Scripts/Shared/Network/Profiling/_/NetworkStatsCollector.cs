using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

namespace PeacefulHills.Network.Profiling
{
    public class NetworkStatsCollector : INetworkStatsCollector
    {
        private readonly Dictionary<string, NetworkStatsEntry> _entries;

        public NetworkStatsCollector()
        {
            _entries = new Dictionary<string, NetworkStatsEntry>();
        }
        
        public IEnumerable<byte[]> IterateSendBytes()
        {
            foreach (string name in _entries.Keys)
            {
                NativeArray<byte> entryBytes = _entries[name].AsNativeArray();
                var nameBytes = new NativeArray<byte>(Encoding.UTF8.GetBytes(name), Allocator.Temp);
                var sendBytes = new NativeArray<byte>(sizeof(int) + nameBytes.Length + entryBytes.Length, Allocator.Temp);
                
                var bytesWriter = new DataStreamWriter(sendBytes);
                bytesWriter.WriteInt(name.Length);
                bytesWriter.WriteBytes(nameBytes);
                bytesWriter.WriteBytes(entryBytes);
                
                yield return sendBytes.ToArray();

                nameBytes.Dispose();
                sendBytes.Dispose();
            }
        }

        public NetworkStatsEntry Entry(string name)
        {
            if (_entries.ContainsKey(name))
            {
                return _entries[name];
            }
            return _entries[name] = new NetworkStatsEntry(32);
        }

        public void Clear()
        {
            foreach (NetworkStatsEntry entry in _entries.Values)
            {
                entry.Clear();
            }
        }

        public void Dispose()
        {
            foreach (NetworkStatsEntry entry in _entries.Values)
            {
                entry.Dispose();
            }
            _entries.Clear();
        }
    }
}