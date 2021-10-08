namespace PeacefulHills.Bootstrap
{
    public abstract class Bootstrap : Boot
    {
        public class ActInstruction : IBootInstruction
        {
            public void Apply(Boot boot)
            {
                if (boot is Bootstrap bootstrap)
                {
                    bootstrap.Act();
                }
            }
        }
        
        public override void Setup()
        {
            base.Setup();
            Instructions.Add(new ActInstruction());
        }

        protected abstract void Act();
    }
}