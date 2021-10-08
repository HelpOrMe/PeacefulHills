using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PeacefulHills.Bootstrap
{
    public static class BootExtensions
    {
        public static void Setup(this IEnumerable<Boot> boots)
        {
            foreach (Boot bootstrap in boots)
            {
                bootstrap.Setup();
            }
        }
        
        public static void Call(this IEnumerable<Boot> boots)
        {
            foreach (Boot bootstrap in boots)
            {
                bootstrap.Call();
            }
        }

        public static void Propagate(this IEnumerable<Boot> boots, Action<Boot> action)
        {
            foreach (Boot boot in boots)
            {
                action(boot);
                boot.Children.Propagate(action);
            }
        }
        
        public static bool TryFind<T>(this IEnumerable<IBootInstruction> instructions, out T instruction) 
            where T : IBootInstruction
        {
            instruction = (T) instructions.FirstOrDefault(i => i is T);
            return instruction != null;
        }

        public static bool TryFind(this IEnumerable<IBootInstruction> instructions, Type type,
            out IBootInstruction instruction)
        {
            instruction = instructions.FirstOrDefault(type.IsInstanceOfType);
            return instruction != null;
        }
    }
}