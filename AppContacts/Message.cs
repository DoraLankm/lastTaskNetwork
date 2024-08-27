using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppContacts
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;

        public int RecepentId { get; set; } = -1;

        public int? SenderId { get; set; }

        public Command Command { get; set; } = Command.None;

        public IEnumerable<User> Users { get; set; } = [];

    }

    public enum Command
    {
        None,
        Register,
        Exit,
        Users,
        Confirm
    }
   
}
