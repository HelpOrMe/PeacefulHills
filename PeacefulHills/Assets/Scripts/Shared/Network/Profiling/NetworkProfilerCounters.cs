using Unity.Profiling;

namespace PeacefulHills.Network.Profiling
{
    public static class NetworkProfilerCounters
    {
        public static readonly ProfilerCounterValue<int> BytesSent =
            new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "Network.Sent", 
                                          ProfilerMarkerDataUnit.Bytes, 
                                          ProfilerCounterOptions.None);
        
        public static readonly ProfilerCounterValue<int> BytesReceived =
            new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "Network.Received", 
                                          ProfilerMarkerDataUnit.Bytes, 
                                          ProfilerCounterOptions.None);
    }
}
