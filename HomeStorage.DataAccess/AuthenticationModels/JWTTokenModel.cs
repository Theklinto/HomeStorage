using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeStorage.DataAccess.AuthenticationModels
{
    public class JwtTokenModel
    {
        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [JsonPropertyName("expiration")]
        public DateTime? Expiration { get; set; }
    }
}
