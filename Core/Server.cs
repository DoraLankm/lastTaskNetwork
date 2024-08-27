using AppContacts;
using Infrastructure.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core
{
    public class Server : ChatBase
    {
        private readonly IMessageSource _source;
        HashSet<User> _users = new HashSet<User>();
        public Server(IMessageSource source)
        {
            _source = source;
        }

        public override async Task Start()
        {
            await Task.CompletedTask;
            Task.Run(Listener);
        }

        protected override async Task Listener()
        {
            while (CancellationToken.IsCancellationRequested)
            {
                try
                {
                    ReceiveResult result = await _source.Receive(CancellationToken);
                    if (result.Message is null) throw new Exception("Message is null");


                    switch(result.Message.Command)
                    {
                        case Command.None:

                            break;
                        case Command.Register:
                            await JoinHandler(result);
                            break;
                        case Command.Exit:
                            break;
                        case Command.Users:
                            break;
                        case Command.Confirm:
                            break;
                    }

                    await Console.Out.WriteAsync($"{JsonSerializer.Serialize(result.Message)}");
                }
                catch (Exception e)
                {
                    await Console.Out.WriteAsync(e.Message);
                }
            }
        }

        private async Task JoinHandler(ReceiveResult result)
        {
            User user = _users.FirstOrDefault(u => u.Name == result.Message!.Text);
            if (user is null)
            {
                user = new User { Name = result.Message!.Text, EndPoint = result.EndPoint };
                _users.Add(user);
            }

            user.EndPoint = result.EndPoint;

            await _source.Send( new Message() {Command = Command.Register, RecepentId = user.Id}, user.EndPoint!, CancellationToken);

            await _source.Send(new Message() { Command = Command.Users, RecepentId = user.Id, Users = _users }, user.EndPoint!, CancellationToken);

            await SendAllAsync(new Message() { Command = Command.Confirm, Text = $"{user.Name} joined to chat" });
        }

        private async Task MessageHandler(ReceiveResult result)
        {
            if (result.Message!.RecepentId < 0)
            {
                await SendAllAsync(result.Message);
            }

            else
            {
                await _source.Send(result.Message, 
                    _users.First(u => u.Id == result.Message.SenderId).EndPoint!, 
                    CancellationToken);

                var recipientEndPoint = _users.FirstOrDefault(u => u.Id == result.Message.SenderId)?.EndPoint;
                if (recipientEndPoint is not null)
                {
                    await _source.Send(result.Message,
                        _users.First(u => u.Id == result.Message.SenderId).EndPoint!,
                        CancellationToken);
                }
            }
        }

        private async Task SendAllAsync(Message message)
        {
            foreach (var user in _users)
            {
                await _source.Send(message, user.EndPoint!, CancellationToken);
            }

        }
    }

    
}
