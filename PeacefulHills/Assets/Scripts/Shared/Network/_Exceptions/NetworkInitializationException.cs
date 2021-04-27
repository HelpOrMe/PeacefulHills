using System;

namespace PeacefulHills.Network.Exceptions
{
    public class NetworkInitializationException : Exception
    {
        public NetworkInitializationException(string message) : base(message) { }
    }
}