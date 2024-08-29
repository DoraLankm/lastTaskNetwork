using Domain;
using System.Net;
using System.Text.Json.Serialization;

namespace AppContacts
{
    public record User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime lastOnline = DateTime.Now;

        [JsonIgnore]
        public IPEndPoint? EndPoint { get; set; }

        public static User FromDomain(UserEntity userEntity) => new User()
        {
            Id = userEntity.Id,
            Name = userEntity.Name,
            lastOnline = userEntity.lastOnline
        };

    }
}


