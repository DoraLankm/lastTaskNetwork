using AppContacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Provider
{
    public interface IMessageSource
    {
        Task<ReceiveResult> Receive(CancellationToken cancellationToken);
        Task Send(Message msg, IPEndPoint endPoint, CancellationToken cancellationToken);
        //IPEndPoint CreateEndPoint(string address, int port);
        //IPEndPoint GetServerEndpoint();
    }
}
