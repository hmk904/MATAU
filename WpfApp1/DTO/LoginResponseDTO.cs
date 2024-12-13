using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfApp1.DTO
{
    public class LoginResponseDTO
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }

        [JsonPropertyName("roles")]
        public string Roles { get; set; }
    }
}
