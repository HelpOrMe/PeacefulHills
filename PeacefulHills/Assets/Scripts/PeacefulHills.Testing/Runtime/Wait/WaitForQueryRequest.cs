using System.Diagnostics;
using System.Threading.Tasks;
using Unity.Entities;

namespace PeacefulHills.Testing
{
    public class WaitForQueryRequest : IWaitRequest
    {
        public readonly EntityQueryDesc QueryDesc;
        public readonly TaskCompletionSource<EntityQuery> TaskCompletionSource;
        public readonly int TimeoutMs;
        private readonly Stopwatch _stopwatch;
        private EntityQuery _query;

        public WaitForQueryRequest(EntityQueryDesc queryDesc, TaskCompletionSource<EntityQuery> taskCompletionSource, 
                                   int timeoutMs)
        {
            QueryDesc = queryDesc;
            TaskCompletionSource = taskCompletionSource;
            TimeoutMs = timeoutMs;
            _stopwatch = Stopwatch.StartNew();
        }
        
        public bool Update(World world)
        {
            if (!world.EntityManager.IsQueryValid(_query))
            {
                _query = world.EntityManager.CreateEntityQuery(QueryDesc);
            }

            if (!_query.IsEmpty || _stopwatch.ElapsedMilliseconds > TimeoutMs)
            {
                _stopwatch.Stop();
                TaskCompletionSource.SetResult(_query);
                return false;
            }

            return true;
        }
    }
}