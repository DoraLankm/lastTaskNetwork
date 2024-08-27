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
    public class Client : ChatBase
    {
        private readonly IMessageSource _source;
        private IPEndPoint _serverEndPoint;
        private readonly User _username;
        private IEnumerable<User> _users = [];
        public Client(string name,IPEndPoint serverEndpoint,IMessageSource source)
        {
            _source = source;
            _serverEndPoint = serverEndpoint;
            _username = new User {Name = name } ;
            
        }

        public override async Task Start()
        {
            
            var join = new Message {Text = $"{_username.Name}", Command = Command.Register };
            await _source.Send(join, _serverEndPoint, CancellationToken);

            Task.Run(Listener);
            while (!CancellationToken.IsCancellationRequested)
            {
                string input = (await Console.In.ReadLineAsync()) ?? string.Empty;
                Message message = new() { Text = input, SenderId = _username.Id };
                await _source.Send(message,_serverEndPoint, CancellationToken);
            }
        }

        protected override async Task Listener()
        {
            while (!CancellationToken.IsCancellationRequested)
            {
                try
                {
                    ReceiveResult? result = await _source.Receive(CancellationToken);
                    if (result.Message is null) throw new Exception("Message is null");

                    if (result.Message.Command == Command.Register)
                    {
                        JoinHandler(result.Message);
                    }
                    else if (result.Message.Command == Command.Users)
                    {
                        UsersHandler(result.Message);
                    }

                    else if (result.Message.Command == Command.None)
                    {
                        MessageHandler(result.Message);
                    }

                    await Console.Out.WriteAsync($"{JsonSerializer.Serialize(result.Message)}");
                }
                catch(Exception e)
                {
                    await Console.Out.WriteAsync(e.Message);
                }
            }
        }

        private void MessageHandler(Message message)
        {
            Console.WriteLine($"{_users.First(u => u.Id == message.SenderId)} {message.Text}");
        }

        private void UsersHandler(Message message)
        {
            _users = message.Users;
        }

        private void JoinHandler(Message message)
        {
            _username.Id  = message.RecepentId;
            Console.WriteLine("Registraction is success");
        }
    }
}
