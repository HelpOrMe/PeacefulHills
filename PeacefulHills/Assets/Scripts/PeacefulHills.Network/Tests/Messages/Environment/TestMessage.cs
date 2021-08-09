using System;
using NUnit.Framework.Internal;
using PeacefulHills.Network.Messages;
using Unity.Entities;

namespace PeacefulHills.Network.Tests
{
    public struct TestMessage : IMessage, IComponentData
    {
        public byte Value1;
        public short Value2;
        public uint Value3;
        public ulong Value4;

        public static TestMessage Random()
        {
            var randomizer = Randomizer.CreateRandomizer();
            return new TestMessage
            {
                Value1 = randomizer.NextByte(),
                Value2 = randomizer.NextShort(),
                Value3 = randomizer.NextUInt(),
                Value4 = randomizer.NextULong()
            };
        }
        
        public override bool Equals(object obj)
        {
            return obj is TestMessage other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Value1.GetHashCode();
                hashCode = (hashCode * 397) ^ Value2.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Value3;
                hashCode = (hashCode * 397) ^ Value4.GetHashCode();
                return hashCode;
            }
        }

        public bool Equals(TestMessage other)
        {
            return Value1 == other.Value1 && Value2 == other.Value2 && Value3 == other.Value3 && Value4 == other.Value4;
        }

        public override string ToString()
        {
            return $"{Value1} {Value2} {Value3} {Value4}";
        }
    }
}