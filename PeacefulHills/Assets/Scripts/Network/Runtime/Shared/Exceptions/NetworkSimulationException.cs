using System;

namespace PeacefulHills.Network
{
    public class NetworkSimulationException : Exception
    {
        public NetworkSimulationException(string message) : base(message)
        {
        }
    }
}