using System;
using PeacefulHills.Bootstrap;
using PeacefulHills.Bootstrap.Controls;

namespace PeacefulHills.Network.Profiling
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class SplitWorldsConditionAttribute : BootstrapConditionAttribute
    {
        public override bool Check(WorldBootstrapInfo bootstrapInfo)
        { 
            return NetworkWorldsInitSettings.Current.SplitWorlds;
        }
    }
}