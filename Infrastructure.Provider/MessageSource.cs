using AppContacts;
using System.Net;
using System.Net.Sockets;
using AppContracts;
using AppContracts.Extencions;
namespace Infrastructure.Provider

{

    
    public class MessageSource : IMessageSource
    {
        private readonly UdpClient _udpClient;
        public MessageSource(UdpClient udpClient)
        {
            _udpClient = udpClient;
        }
        public async Task<ReceiveResult> Receive(CancellationToken cancellationToken)
        {
            var data = await _udpClient.ReceiveAsync();
            return new(data.RemoteEndPoint, data.Buffer.ToMessage());
        }

        public async Task Send(Message msg, IPEndPoint endPoint, CancellationToken cancellationToken)
        {
            await _udpClient.SendAsync(msg.ToBytes(), endPoint, cancellationToken);
        }
    }
}


