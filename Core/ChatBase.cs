using System.Net;

namespace Core
{
    public abstract class ChatBase
    {
        private readonly IPEndPoint _endpoint;
        protected ChatBase(IPEndPoint endPoint)
        {
            _endpoint = endPoint;
        }
    }
}
