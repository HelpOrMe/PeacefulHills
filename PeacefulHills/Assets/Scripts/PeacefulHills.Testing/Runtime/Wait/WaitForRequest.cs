using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Unity.Entities;

namespace PeacefulHills.Testing
{
    public class WaitForRequest : IWaitRequest
    {
        public readonly Func<bool> Condition;
        public readonly TaskCompletionSource<bool> TaskCompletionSource;
        public readonly int TimeoutMs;
        private readonly Stopwatch _stopwatch;

        public WaitForRequest(Func<bool> condition, TaskCompletionSource<bool> taskCompletionSource, int timeoutMs)
        {
            Condition = condition;
            TaskCompletionSource = taskCompletionSource;
            TimeoutMs = timeoutMs;
            _stopwatch = Stopwatch.StartNew();
        }
        
        public bool Update(World world)
        {
            if (Condition() || _stopwatch.ElapsedMilliseconds >= TimeoutMs)
            {
                _stopwatch.Stop();
                TaskCompletionSource.SetResult(true);
                return false;
            }
            
            return true;
        }
    }
}