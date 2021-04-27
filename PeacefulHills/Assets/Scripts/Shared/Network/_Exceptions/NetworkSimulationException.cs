using System;

namespace PeacefulHills.Network.Exceptions
{
    public class NetworkSimulationException : Exception
    {
        public NetworkSimulationException(string message) : base(message) { }
    }
}