using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace PeacefulHills.Testing
{
    public class WorldsException : ResultStateException
    {
        public WorldsException(string message) : base(message) { }

        public override ResultState ResultState => ResultState.Error;
    }
}