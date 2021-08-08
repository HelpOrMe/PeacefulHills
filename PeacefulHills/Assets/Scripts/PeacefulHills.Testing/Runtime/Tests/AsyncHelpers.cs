using System;
using System.Threading;
using System.Threading.Tasks;

namespace PeacefulHills.Testing
{
    public static class AsyncHelpers
    {
        public static EnumerableSynchronizationContext RunSync(Func<Task> task)
        {
            SynchronizationContext oldContext = SynchronizationContext.Current;
            var context = new EnumerableSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(context);
            context.Post(async _ =>
            {
                try
                {
                    await task();
                }
                catch (Exception e)
                {
                    context.InnerException = e;
                    throw;
                }
                finally
                {
                    context.EndMessageLoop();
                }
            }, null);
            
            SynchronizationContext.SetSynchronizationContext(oldContext);

            return context;
        }
    }
}