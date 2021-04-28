using System;

namespace PeacefulHills.Network
{
    public class NetworkInitializationException : Exception
    {
        public NetworkInitializationException(string message) : base(message) { }
    }
}