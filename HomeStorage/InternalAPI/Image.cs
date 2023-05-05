using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.InternalAPI
{
    public static partial class HSAPI
    {
        public static class Image
        {
            private static readonly string _url = BaseAPIUrl.TrimEnd('/') + "/image";
            public static Uri GetUri(Guid imageId) => new($"{_url}/{imageId}");
            public static ImageSource GetSource(Guid imageId) => ImageSource.FromUri(GetUri(imageId));
        }
    }
}
