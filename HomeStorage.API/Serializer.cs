using System.Text.Json;

namespace HomeStorage.API
{
    public static class Serializer
    {
        public static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new(JsonSerializerDefaults.Web);
    }
}
