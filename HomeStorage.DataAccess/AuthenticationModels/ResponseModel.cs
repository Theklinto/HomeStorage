using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.DataAccess.AuthenticationModels
{
    public partial class ResponseModel
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
