using System.Threading.Tasks;
using Unity.Entities;

namespace PeacefulHills.Testing
{
    public class WaitFramesRequest : IWaitRequest
    {
        public int Frames;
        public readonly TaskCompletionSource<bool> TaskCompletionSource;

        public WaitFramesRequest(int frames, TaskCompletionSource<bool> taskCompletionSource)
        {
            Frames = frames;
            TaskCompletionSource = taskCompletionSource;
        }
        
        public bool Update(World world)
        {
            if (Frames-- == 0)
            {
                TaskCompletionSource.SetResult(true);
                return false;
            }
            return true;
        }
    }
}