using System;
using System.Collections.Generic;

namespace PeacefulHills.Bootstrap
{
    public abstract class Boot
    {
        /// <summary>
        /// Nested boot, by default, will be called by the instruction.
        /// </summary>
        public readonly List<Boot> Children = new List<Boot>();

        /// <summary>
        /// The set of instructions that will be applied in order when calling the boot.
        /// <see cref="Call"/>
        /// </summary>
        public readonly List<IBootInstruction> Instructions = new List<IBootInstruction>();
        
        /// <summary>
        /// Called before <see cref="Children"/> was filled, used to setup instructions, do not add custom logic here. 
        /// </summary>
        public virtual void Setup()
        {
            Instructions.AddRange(GetInstructionsFromAttributes());
            Instructions.Add(new CallChildrenInstruction());
        }

        protected IEnumerable<IBootInstruction> GetInstructionsFromAttributes()
        {
            Type type = GetType();

            foreach (object attribute in type.GetCustomAttributes(true))
            {
                if (attribute is IBootInstruction instruction)
                {
                    yield return instruction;
                }
            }
        }
        
        public void Call()
        {
            foreach (IBootInstruction instruction in Instructions)
            {
                instruction.Apply(this);
            }
        }
    }
}