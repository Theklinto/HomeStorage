using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage
{
    public enum EStorageKey
    {
        Undefined = 0,

        //User data
        Username = 1,
        Email = 2,
        Password = 3,

        JwtToken = 20,
        JwtTokenExpiration = 21
    }
    public static class SecureStorageExtension
    {

        public static async Task SetAsync(EStorageKey key, string value) => await SecureStorage.Default.SetAsync(Enum.GetName(key.GetType(), key), value);
        public static async Task<string> GetAsync(EStorageKey key) => await SecureStorage.Default.GetAsync(Enum.GetName(key.GetType(), key));
    }
}
