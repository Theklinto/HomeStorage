using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Models.AuthenticationModels
{
    public class TokenModel
    {
        public required string Token { get; set; }
        public required DateTime Expiration { get; set; }
    }
}
