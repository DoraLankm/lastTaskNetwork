using AppContacts;
using System.Net;
using System.Net.Sockets;
using AppContracts;
using AppContracts.Extencions;
namespace Infrastructure.Provider

{

    public interface IMessageSource
    {
        Task<Message?> Receive(CancellationToken cancellationToken);
        Task Send(Message msg, IPEndPoint endPoint, CancellationToken cancellationToken);
        //IPEndPoint CreateEndPoint(string address, int port);
        //IPEndPoint GetServerEndpoint();
    }
    public class MessageSource : IMessageSource
    {
        private readonly UdpClient _udpClient;
        public MessageSource(UdpClient udpClient)
        {
            _udpClient = udpClient;
        }
        public async Task<Message?> Receive(CancellationToken cancellationToken)
        {
            var data = await _udpClient.ReceiveAsync();
            return data.Buffer.ToMessage();
        }

        public async Task Send(Message msg, IPEndPoint endPoint, CancellationToken cancellationToken)
        {
            await _udpClient.SendAsync(msg.ToBytes(), endPoint, cancellationToken);
        }
    }
}
