using System.Threading;

namespace TestEventService.Extensions
{
    public static class CancellationTokenSourceExtensions
    {
        public static void CancelAndDispose(this CancellationTokenSource self)
        {
            self.Cancel();
            self.Dispose();
        }
    }
}