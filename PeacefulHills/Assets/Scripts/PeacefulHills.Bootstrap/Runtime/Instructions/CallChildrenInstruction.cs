namespace PeacefulHills.Bootstrap
{
    public class CallChildrenInstruction : IBootInstruction
    {
        public void Apply(Boot boot)
        {
            foreach (Boot child in boot.Children)
            {
                child.Call();
            }
        }
    }
}