using AppContacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppContracts.Extencions
{
    public static class MessageExtensions
    {
        public static Message? ToMessage(this byte[] data)
        {
            return JsonSerializer.Deserialize<Message>(Encoding.UTF8.GetString(data));
        }

        public static byte[] ToBytes(this Message message)
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        }
    }
}
