using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Models
{
    public class JsonWithFileModel
    {
        public string Json { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
    }
}
