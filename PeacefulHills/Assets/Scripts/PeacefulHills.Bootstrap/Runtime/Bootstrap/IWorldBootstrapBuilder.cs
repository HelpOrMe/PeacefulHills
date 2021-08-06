using System.Collections.Generic;

namespace PeacefulHills.Bootstrap
{
    public interface IWorldBootstrapBuilder
    {
        List<WorldBootstrapContext> BuildBootstrapContexts();
    }
}