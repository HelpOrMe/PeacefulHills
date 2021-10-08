using System;
using System.Collections.Generic;

namespace PeacefulHills.Extensions
{
    public static class PairExtensions
    {
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> source, 
            out TKey key, out TValue value)
        {
            key = source.Key;
            value = source.Value;
        }
        
        public static void Deconstruct<T0, T1>(this Tuple<T0, T1> source, 
            out T0 item1, out T1 item2)
        {
            item1 = source.Item1;
            item2 = source.Item2;
        }
    }
}