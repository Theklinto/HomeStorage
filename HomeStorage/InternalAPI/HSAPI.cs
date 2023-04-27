using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.InternalAPI
{
    public static partial class HSAPI
    {
        private static HttpClient _client = new();
        private const string BaseAPIUrl = "http://192.168.1.181:45455/api/";
    }
}
