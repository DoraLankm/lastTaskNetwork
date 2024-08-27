using System.Net;
using System.Text.Json.Serialization;

namespace AppContacts
{
    public record User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public IPEndPoint? EndPoint { get; set; }


    }
}


