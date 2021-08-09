using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

// https://social.msdn.microsoft.com/Forums/en-US/163ef755-ff7b-4ea5-b226-bbe8ef5f4796/is-there-a-pattern-for-calling-an-async-method-synchronously?forum=async

namespace PeacefulHills.Testing
{
    public class EnumeratorSynchronizationContext : SynchronizationContext
    {
        private bool _done;
        public Exception InnerException { get; set; }
        private readonly AutoResetEvent _workItemsWaiting = new AutoResetEvent(false);

        private readonly Queue<Tuple<SendOrPostCallback, object>> _items =
            new Queue<Tuple<SendOrPostCallback, object>>();

        public override void Send(SendOrPostCallback d, object state)
        {
            throw new NotSupportedException("We cannot send to our same thread");
        }

        public void EndMessageLoop()
        {
            Post(_ => _done = true, null);
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            lock (_items)
            {
                _items.Enqueue(Tuple.Create(d, state));
            }
            _workItemsWaiting.Set();
        }

        public IEnumerator BeginMessageLoop()
        {
            while (!_done)
            {
                Tuple<SendOrPostCallback, object> task = null;
                lock (_items)
                {
                    if (_items.Count > 0)
                    {
                        task = _items.Dequeue();
                    }
                }
                if (task != null)
                {
                    task.Item1(task.Item2);
                    if (InnerException != null)
                    {
                        throw InnerException;
                    }
                }

                yield return null;
            }
        }

        public override SynchronizationContext CreateCopy()
        {
            return this;
        }
    }
}