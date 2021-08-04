using Unity.Profiling;

namespace PeacefulHills.Network.Messages.Profiling
{
    public static class MessagesProfilerCounters
    {
        public static readonly ProfilerCounterValue<int> BytesSent =
            new ProfilerCounterValue<int>(ProfilerCategory.Scripts, "Network.Messages.Sent",
                                          ProfilerMarkerDataUnit.Bytes,
                                          ProfilerCounterOptions.None);
    }
}