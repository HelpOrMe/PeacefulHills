using System;

namespace PeacefulHills.Bootstrap
{
    public class ManualInstruction : IBootInstruction
    {
        public readonly Action<Boot> Action;

        public ManualInstruction(Action<Boot> action)
        {
            Action = action;
        }

        public void Apply(Boot boot)
        {
            Action(boot);
        }
    }
}