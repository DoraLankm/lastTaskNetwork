using AppContacts;
using Infrastructure.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core
{
    public class Client 
    {
        private readonly CancellationToken _token;
        private readonly IMessageSource _source;
        private List<User> _users;
        public Client(IPEndPoint serverEndpoint,IMessageSource source, CancellationToken token)
        {
            _token = token;
            _source = source;

            
        }

        public async Task Start()
        {
            Task.Run(Listener);
            while(!_token.IsCancellationRequested)
            {
                string input = (await Console.In.ReadLineAsync()) ?? string.Empty;
                Message message = new() { Text = input };
                await _source.Send(message,_token);
            }
        }

        public async Task Listener()
        {
            while (!_token.IsCancellationRequested)
            {
                try
                {
                    Message? msg = await _source.Receive(_token);
                    if (msg is null) throw new Exception("Message is null");

                    await Console.Out.WriteAsync($"{JsonSerializer.Serialize(msg)}");
                }
                catch(Exception e)
                {

                }
            }
        }
    }
}
