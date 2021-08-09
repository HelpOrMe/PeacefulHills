using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace PeacefulHills.Testing
{
    public static class AsyncSupport
    {
        public static IEnumerator RunAsEnumerator(Func<Task> task)
        {
            SynchronizationContext oldContext = SynchronizationContext.Current;
            var newContext = new EnumeratorSynchronizationContext();
            
            SynchronizationContext.SetSynchronizationContext(newContext);
            
            newContext.Post(async _ => 
            {
                try
                {
                    await task();
                }
                catch (Exception e)
                {
                    newContext.InnerException = e;
                    throw;
                }
                finally
                {
                    newContext.EndMessageLoop();
                }
            }, null);
            
            SynchronizationContext.SetSynchronizationContext(oldContext);
            
            return newContext.BeginMessageLoop();
        }
    }
}