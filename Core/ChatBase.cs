using System.Net;

namespace Core
{
    public abstract class ChatBase
    {
        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
        protected CancellationToken CancellationToken => CancellationTokenSource.Token;
        protected abstract Task Listener();
        public abstract Task Start();
    }
}
