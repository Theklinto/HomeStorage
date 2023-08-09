using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HomeStorage.DataAccess.AuthenticationModels;
using Newtonsoft.Json;
using DeviceStorage = HomeStorage.SecureStorageExtension;

namespace HomeStorage.InternalAPI
{
    public static partial class HSAPI
    {
        public static partial class Authentication
        {
            private static string _url { get; set; } = BaseAPIUrl + "auth/";
            public static async Task<JwtTokenModel> Login(LoginModel model)
            {
                HttpResponseMessage httpResponse = await _client.PostAsJsonAsync(_url + nameof(Login), model);
                if (httpResponse.IsSuccessStatusCode)
                {
                    JwtTokenModel jwtTokenModel = JsonConvert.DeserializeObject<JwtTokenModel>(await httpResponse.Content.ReadAsStringAsync());
                    return jwtTokenModel;
                }

                return new();
            }

            public static async Task<ResponseModel> Register(RegisterModel model)
            {
                HttpResponseMessage httpResponse = await _client.PostAsJsonAsync(_url + nameof(Register), model);
                
                if (httpResponse.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<ResponseModel>(await httpResponse.Content.ReadAsStringAsync());

                return new()
                {
                    Message = $"{(int)httpResponse.StatusCode}: Der skete en ukendt fejl",
                };

            }

            public static void PurgeAuthenticationStorage()
            {
                new List<EStorageKey>()
                {
                    EStorageKey.Username,
                    EStorageKey.Password,
                    EStorageKey.Email,
                    EStorageKey.JwtToken,
                    EStorageKey.JwtTokenExpiration
                }.ForEach(DeviceStorage.Remove);
            }
        }
    }
}
